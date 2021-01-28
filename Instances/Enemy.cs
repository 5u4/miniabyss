using Godot;
using MiniAbyss.Data;

namespace MiniAbyss.Instances
{
    public class Enemy : Creature
    {
        public static readonly NodePath EnemyScenePath = "res://Instances/Enemy.tscn";

        public static Enemy Make(EnemyKind kind)
        {
            var e = (Enemy) GD.Load<PackedScene>(EnemyScenePath).Instance();
            var data = EnemyData.Dictionary[kind];
            e.Display = data.Display;
            e.Health = data.Health;
            e.Strength = data.Strength;
            e.Defence = data.Defence;
            e.SpriteFramesResource = data.SpriteFrames;
            return e;
        }

        public Vector2 Act()
        {
            // TODO: Add enemy ai
            if (IsDead()) return Vector2.Zero;
            var dir = Mathf.FloorToInt(Rng.Instance.G.Randf() * 4);
            return dir == 0 ? Vector2.Left : dir == 1 ? Vector2.Right : dir == 2 ? Vector2.Up : Vector2.Down;
        }

        public override void OnActionFinished()
        {
            base.OnActionFinished();
            BattleGrid.EmitSignal(nameof(Games.BattleGrid.OneEnemyTurnEndedSignal));
        }
    }
}
