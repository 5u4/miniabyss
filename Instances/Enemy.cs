using Godot;

namespace MiniAbyss.Instances
{
    public class Enemy : Creature
    {
        public Vector2 Act()
        {
            // TODO: Add enemy ai
            if (IsDead()) return Vector2.Zero;
            var dir = Mathf.FloorToInt(GD.Randf() * 4);
            return dir == 0 ? Vector2.Left : dir == 1 ? Vector2.Right : dir == 2 ? Vector2.Up : Vector2.Down;
        }

        public override void OnActionFinished()
        {
            base.OnActionFinished();
            BattleGrid.EmitSignal(nameof(Games.BattleGrid.OneEnemyTurnEndedSignal));
        }
    }
}
