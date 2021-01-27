using System.Collections.Generic;
using Godot;
using MiniAbyss.Instances;

namespace MiniAbyss.Games
{
    public class BattleGrid : TileMap
    {
        [Signal]
        public delegate void OnGenerateMap();

        [Export] public NodePath PlayerPath;
        [Export] public NodePath ExitPath;
        public Player Player;
        public Exit Exit;
        public const int MapEnlargeSize = 3;
        public const int WallTile = -1;
        public const int EmptyTile = 0;
        public const int ExitTile = 1;
        public Dictionary<int, Entity> EntityMap;

        public override void _Ready()
        {
            Player = GetNode<Player>(PlayerPath);
            Exit = GetNode<Exit>(ExitPath);
        }

        public override void _Process(float delta)
        {
            base._Process(delta);
            // TODO: Remove debug
            if (Input.IsActionJustPressed("ui_accept")) Generate(5, 1, 0.5f);
        }

        public void Generate(int dim, int offset, float coverage)
        {
            EntityMap = new Dictionary<int, Entity>();
            GD.Randomize();
            var m = CrawlEmptySpaces(dim, coverage);
            SetTilesWithMap(m, dim, offset);
            PlacePlayer();
            PlaceExitAwayFrom(WorldToMap(Player.Position));
            CenterGridInViewport();
            EmitSignal(nameof(OnGenerateMap));
        }

        public void HandleAction(Entity e, Vector2 dir)
        {
            if (IsWall(WorldToMap(e.Position) + dir))
            {
                e.Bump();
                return;
            }
            e.Move(dir);
        }

        private bool IsWall(Vector2 v)
        {
            return GetCellv(v) == WallTile
                   || GetCellv(v + new Vector2(1, 1)) == WallTile
                   || GetCellv(v + new Vector2(1, -1)) == WallTile
                   || GetCellv(v + new Vector2(-1, 1)) == WallTile
                   || GetCellv(v + new Vector2(-1, -1)) == WallTile;
        }

        private void PlacePlayer()
        {
            var cells = GetUsedCells();
            var cell = (Vector2) cells[Mathf.FloorToInt(GD.Randf() * cells.Count)];
            while (IsWall(cell)) cell = (Vector2) cells[Mathf.FloorToInt(GD.Randf() * cells.Count)];
            Player.Position = MapToWorld(cell);
            EntityMap[GridPosToEntityMapKey(cell)] = Player;
        }

        private void PlaceExitAwayFrom(Vector2 v)
        {
            var queue = new Queue<Vector2>();
            queue.Enqueue(v);
            var visited = new HashSet<int>();
            var lastPos = v;
            while (queue.Count > 0)
            {
                var size = queue.Count;
                for (var i = 0; i < size; i++)
                {
                    var pos = queue.Dequeue();
                    var key = GridPosToEntityMapKey(pos);
                    if (visited.Contains(key)) continue;
                    lastPos = pos;
                    visited.Add(key);
                    if (!IsWall(pos + Vector2.Left)) queue.Enqueue(pos + Vector2.Left);
                    if (!IsWall(pos + Vector2.Right)) queue.Enqueue(pos + Vector2.Right);
                    if (!IsWall(pos + Vector2.Up)) queue.Enqueue(pos + Vector2.Up);
                    if (!IsWall(pos + Vector2.Down)) queue.Enqueue(pos + Vector2.Down);
                }
            }

            SetCellv(lastPos, ExitTile);
            Exit.Position = MapToWorld(lastPos);
            EntityMap[GridPosToEntityMapKey(lastPos)] = Exit;
        }

        private int GridPosToEntityMapKey(Vector2 v)
        {
            return Mathf.FloorToInt(v.x * 1000) + Mathf.FloorToInt(v.y);
        }

        private Vector2 EntityMapKeyToGridPos(int id)
        {
            return new Vector2(id / 1000, id % 1000);
        }

        private void CenterGridInViewport()
        {
            var gSize = GetUsedRect().Size * CellSize;
            var vSize = GetViewportRect().Size;
            var xDiff = vSize.x - gSize.x;
            var yDiff = vSize.y - gSize.y;
            var newPos = new Vector2(Position);
            if (xDiff > 0) newPos.x = xDiff / 2;
            if (yDiff <= 0) newPos.y = -yDiff / 2;
            Position = newPos;
        }

        private void SetTilesWithMap(IReadOnlyList<int> m, int dim, int offset)
        {
            for (var i = 0; i < m.Count; i++)
            {
                var r = i % dim;
                var c = Mathf.FloorToInt((float) i / dim);
                for (var ii = 0; ii < MapEnlargeSize; ii++)
                for (var jj = 0; jj < MapEnlargeSize; jj++)
                    SetCell(r * MapEnlargeSize + jj + offset, c * MapEnlargeSize + ii + offset, m[i]);
            }

            UpdateBitmaskRegion(Vector2.Zero, new Vector2(dim * MapEnlargeSize, dim * MapEnlargeSize));
        }

        private int[] CrawlEmptySpaces(int dim, float coverage)
        {
            // Initialize walls
            var m = new int[dim * dim];
            for (var i = 0; i < m.Length; i++) m[i] = WallTile;

            // Compute end state
            var totalNeeded = Mathf.CeilToInt(dim * dim * coverage);
            var n = 1;

            // Define local helpers
            int VToI(Vector2 v) => (int) (v.x * dim + v.y);

            // Make starting point
            int MakeRandAxis() => (int) (GD.Randf() * (dim - 2) + 1);
            var curr = new Vector2(MakeRandAxis(), MakeRandAxis());
            m[VToI(curr)] = EmptyTile;

            void Walk(Vector2 dir)
            {
                var nv = curr + dir;
                if (nv.x < 0 || nv.x >= dim|| nv.y < 0 || nv.y >= dim) return;
                curr = nv;
                var i = VToI(curr);
                if (m[i] != WallTile) return;
                m[i] = EmptyTile;
                n++;
            }

            while (n < totalNeeded)
            {
                var walk = Mathf.FloorToInt(GD.Randf() * 4);
                if (walk == 0) Walk(Vector2.Left);
                else if (walk == 1) Walk(Vector2.Right);
                else if (walk == 2) Walk(Vector2.Up);
                else if (walk == 3) Walk(Vector2.Down);
            }

            return m;
        }
    }
}
