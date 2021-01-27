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
        public Player Player;
        public const int MapEnlargeSize = 3;
        public const int Wall = -1;
        public const int Empty = 0;

        public Vector2 StartingPoint;

        public override void _Ready()
        {
            Player = GetNode<Player>(PlayerPath);
        }

        public override void _Process(float delta)
        {
            base._Process(delta);
            if (Input.IsActionJustPressed("ui_accept")) Generate(5, 1, 0.5f);
        }

        public void Generate(int dim, int offset, float coverage)
        {
            GD.Randomize();
            var m = CrawlEmptySpaces(dim, coverage);
            SetTilesWithMap(m, dim, offset);
            CenterGridInViewport();
            EmitSignal(nameof(OnGenerateMap));
        }

        public void HandleAction(Entity e, Vector2 dir)
        {
            // TODO
            e.Move(dir);
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
            for (var i = 0; i < m.Length; i++) m[i] = Wall;

            // Compute end state
            var totalNeeded = Mathf.CeilToInt(dim * dim * coverage);
            var n = 0;

            // Define local helpers
            int VToI(Vector2 v) => (int) (v.x * dim + v.y);

            // Make starting point
            int MakeRandAxis() => (int) (GD.Randf() * (dim - 2) + 1);
            StartingPoint = new Vector2(MakeRandAxis(), MakeRandAxis());
            var curr = new Vector2(StartingPoint);
            m[VToI(curr)] = Empty;

            void Walk(Vector2 dir)
            {
                var nv = curr + dir;
                if (nv.x < 0 || nv.x >= dim|| nv.y < 0 || nv.y >= dim) return;
                curr = nv;
                var i = VToI(curr);
                if (m[i] != Wall) return;
                m[i] = Empty;
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
