using System;
using System.Collections.Generic;
using Godot;
using MiniAbyss.Data;
using MiniAbyss.Instances;

namespace MiniAbyss.Games
{
    public class BattleGrid : TileMap
    {
        [Signal]
        public delegate void GenerateMapSignal();

        [Signal]
        public delegate void PlayerTurnEndedSignal();

        [Signal]
        public delegate void OneEnemyTurnEndedSignal();

        [Signal]
        public delegate void EnemyTurnEndedSignal();

        [Export] public NodePath EnemiesPath;
        [Export] public NodePath ConsumablesPath;
        [Export] public NodePath PlayerPath;
        [Export] public NodePath ExitPath;
        [Export] public PackedScene HealthPotionScene;

        public const int MapEnlargeSize = 3;
        public const int WallTile = -1;
        public const int EmptyTile = 0;
        public const int ExitTile = 1;
        public const float EnemyAmountToDimensionUpperRatio = 0.1f;
        public const float EnemyAmountToDimensionLowerRatio = 0.4f;
        public const int EnemySpawnMinDistanceBetweenPlayer = 5;
        public const float PotionAmountToDimensionUpperRatio = 0.05f;
        public const float PotionAmountToDimensionLowerRatio = 0.1f;
        public const int PotionSpawnMinDistanceBetweenPlayer = 7;

        public Node2D Enemies;
        public Node2D Consumables;
        public Player Player;
        public Exit Exit;
        public Dictionary<int, Entity> EntityMap;
        public int EnemyEndedCounter;

        public override void _Ready()
        {
            Enemies = GetNode<Node2D>(EnemiesPath);
            Consumables = GetNode<Node2D>(ConsumablesPath);
            Player = GetNode<Player>(PlayerPath);
            Exit = GetNode<Exit>(ExitPath);

            Connect(nameof(PlayerTurnEndedSignal), this, nameof(OnPlayerTurnEnded));
            Connect(nameof(OneEnemyTurnEndedSignal), this, nameof(OnOneEnemyTurnEnded));
        }

        public override void _Process(float delta)
        {
            base._Process(delta);
            // TODO: Remove debug
            if (Input.IsActionJustPressed("ui_accept")) Generate(5, 1, 0.5f);
        }

        public void Generate(int dim, int offset, float coverage)
        {
            foreach (Node child in Enemies.GetChildren()) Enemies.RemoveChild(child);
            EntityMap = new Dictionary<int, Entity>();

            GD.Randomize();
            var m = CrawlEmptySpaces(dim, coverage);
            SetTilesWithMap(m, dim, offset);
            PlacePlayer();
            PlaceExitAwayFrom(WorldToMap(Player.Position));
            var enemyAmount = Mathf.RoundToInt(dim * (EnemyAmountToDimensionLowerRatio +
                                                     GD.Randf() * EnemyAmountToDimensionUpperRatio));
            PlaceEntityAwayFromPlayer(enemyAmount, MakeEnemy, EnemySpawnMinDistanceBetweenPlayer);
            var potionAmount =
                Mathf.RoundToInt(dim * (PotionAmountToDimensionLowerRatio +
                                        GD.Randf() * PotionAmountToDimensionUpperRatio));
            PlaceEntityAwayFromPlayer(potionAmount, MakePotion, PotionSpawnMinDistanceBetweenPlayer);
            CenterGridInViewport();
            EmitSignal(nameof(GenerateMapSignal));
        }

        public void OnPlayerTurnEnded()
        {
            EnemyEndedCounter = Enemies.GetChildCount();
            if (EnemyEndedCounter == 0)
            {
                EmitSignal(nameof(EnemyTurnEndedSignal));
                return;
            }
            foreach (Enemy e in Enemies.GetChildren()) HandleAction(e, e.Act());
        }

        public void OnOneEnemyTurnEnded()
        {
            EnemyEndedCounter--;
            if (EnemyEndedCounter <= 0) EmitSignal(nameof(EnemyTurnEndedSignal));
        }

        public void HandleAction(Creature actor, Vector2 dir)
        {
            var srcCell = WorldToMap(actor.Position);
            var destCell = srcCell + dir;
            if (IsWall(destCell))
            {
                actor.Bump();
                return;
            }

            var srcCellKey = GridPosToEntityMapKey(srcCell);
            var destCellKey = GridPosToEntityMapKey(destCell);
            var canMove = true;
            if (EntityMap.ContainsKey(destCellKey)) canMove = EntityInteraction(actor, EntityMap[destCellKey]);

            if (!canMove) return;
            actor.Move(dir);
            if (srcCellKey == destCellKey) return;
            EntityMap.Remove(srcCellKey);
            EntityMap[destCellKey] = actor;
        }

        public void RemoveEntity(Entity e)
        {
            var key = GridPosToEntityMapKey(WorldToMap(e.Position));
            if (EntityMap.ContainsKey(key)) EntityMap.Remove(key);
        }

        private bool EntityInteraction(Creature actor, Entity target)
        {
            var isPlayerAction = actor is Player;
            switch (target)
            {
                case Exit _ when isPlayerAction:
                    GD.Print("Exit"); // TODO: Handle exit
                    return true;
                case Exit _:
                    actor.Bump();
                    return false;
                case Consumable consumable when isPlayerAction:
                    consumable.Consume(Player);
                    var consumableKey = GridPosToEntityMapKey(WorldToMap(consumable.Position));
                    EntityMap.Remove(consumableKey);
                    return true;
                case Consumable _:
                    actor.Bump();
                    return false;
                case Creature creature when creature.Faction == actor.Faction:
                    actor.Bump();
                    return false;
                case Creature creature:
                    actor.Attack(creature);
                    return false;
            }

            return false;
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

        private void PlaceEntityAwayFromPlayer(int amount, Func<Entity> makeEntity, int minDist)
        {
            var playerPos = WorldToMap(Player.Position);
            var emptyCells = GetUsedCells();
            for (var i = 0; i < amount; i++)
            {
                var p = (Vector2) emptyCells[Mathf.FloorToInt(GD.Randf() * emptyCells.Count)];
                var key = GridPosToEntityMapKey(p);
                while (IsWall(p) || EntityMap.ContainsKey(key) ||
                       DistanceBetweenOver(p, playerPos, minDist) < minDist)
                {
                    p = (Vector2) emptyCells[Mathf.FloorToInt(GD.Randf() * emptyCells.Count)];
                    key = GridPosToEntityMapKey(p);
                }

                var e = makeEntity();
                e.Position = MapToWorld(p);
                EntityMap[GridPosToEntityMapKey(p)] = e;
            }
        }

        private Enemy MakeEnemy()
        {
            var enemyKinds = new[] {EnemyKind.Skull}; // TODO generate kinds based on level
            var kind = enemyKinds[Mathf.FloorToInt(GD.Randf() * enemyKinds.Length)];
            var e = Enemy.Make(kind);
            e.BattleGridPath = GetPath();
            Enemies.AddChild(e);
            return e;
        }

        private Consumable MakePotion()
        {
            var p = (HealthPotion) HealthPotionScene.Instance();
            p.BattleGridPath = GetPath();
            p.SpriteFramesResource = GD.Load("res://SpriteFrames/HealthPotionSpriteFrames.tres");
            Consumables.AddChild(p);
            return p;
        }

        private int DistanceBetweenOver(Vector2 v1, Vector2 v2, int maxCap)
        {
            var queue = new Queue<Vector2>();
            queue.Enqueue(v1);
            var visited = new HashSet<int>();
            var dist = 0;
            while (queue.Count > 0 && dist < maxCap)
            {
                var size = queue.Count;
                for (var i = 0; i < size; i++)
                {
                    var pos = queue.Dequeue();
                    if (pos.Equals(v2)) return dist;
                    var key = GridPosToEntityMapKey(pos);
                    if (visited.Contains(key)) continue;
                    visited.Add(key);
                    if (!IsWall(pos + Vector2.Left)) queue.Enqueue(pos + Vector2.Left);
                    if (!IsWall(pos + Vector2.Right)) queue.Enqueue(pos + Vector2.Right);
                    if (!IsWall(pos + Vector2.Up)) queue.Enqueue(pos + Vector2.Up);
                    if (!IsWall(pos + Vector2.Down)) queue.Enqueue(pos + Vector2.Down);
                }

                dist++;
            }

            return dist;
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
