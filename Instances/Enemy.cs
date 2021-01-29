using Godot;
using MiniAbyss.Data;
using MiniAbyss.Hud;

namespace MiniAbyss.Instances
{
    public class Enemy : Creature
    {
        public static readonly NodePath EnemyScenePath = "res://Instances/Enemy.tscn";

        public AnimationPlayer ExpressionAnimationPlayer;
        public RayCast2D RayCast2D;
        public bool Chasing;
        public float SightRadius;

        public override void _Ready()
        {
            base._Ready();
            ExpressionAnimationPlayer = GetNode<AnimationPlayer>("ExpressionAnimationPlayer");
            RayCast2D = GetNode<RayCast2D>("RayCast2D");
        }

        public static Enemy Make(EnemyKind kind)
        {
            var e = (Enemy) GD.Load<PackedScene>(EnemyScenePath).Instance();
            var data = EnemyData.Dictionary[kind];
            e.Display = data.Display;
            e.Health = data.Health;
            e.Strength = data.Strength;
            e.Defence = data.Defence;
            e.SightRadius = data.SightRadius;
            e.SpriteFramesResource = data.SpriteFrames;
            return e;
        }

        public Vector2 Act()
        {
            if (IsDead()) return Vector2.Zero;

            if (!Chasing && PlayerInSight())
            {
                    Chasing = true;
                    ExpressionAnimationPlayer.Play("ExclamationMark");
                    return Vector2.Zero;
            }

            if (Chasing && !PlayerInSight())
            {
                Chasing = false;
                ExpressionAnimationPlayer.Play("QuestionMark");
                return Vector2.Zero;
            }

            return Chasing ? Chase() : Wander();
        }

        public override void OnActionFinished()
        {
            base.OnActionFinished();
            BattleGrid.EmitSignal(nameof(Games.BattleGrid.OneEnemyTurnEndedSignal));
        }

        private Vector2 Wander()
        {
            var dir = Mathf.FloorToInt(Rng.Instance.G.Randf() * 4);
            return dir == 0 ? Vector2.Left : dir == 1 ? Vector2.Right : dir == 2 ? Vector2.Up : Vector2.Down;
        }

        private Vector2 Chase()
        {
            return BattleGrid.ComputeShortestMoveDir(BattleGrid.WorldToMap(Position),
                BattleGrid.WorldToMap(BattleGrid.Player.Position));
        }

        private bool PlayerInSight()
        {
            if (BattleGrid.Player == null) return false;
            var gridDist = BattleGrid.WorldToMap(BattleGrid.Player.Position).DistanceTo(BattleGrid.WorldToMap(Position));
            if (gridDist > SightRadius) return false;
            var dir = (BattleGrid.Player.GlobalPosition - GlobalPosition).Normalized();
            var dist = (BattleGrid.Player.GlobalPosition / 2).DistanceTo(GlobalPosition / 2);
            RayCast2D.CastTo = dir * dist;
            RayCast2D.Enabled = true;
            RayCast2D.ForceRaycastUpdate();
            var blocked = RayCast2D.IsColliding();
            RayCast2D.Enabled = false;
            return !blocked;
        }
    }
}
