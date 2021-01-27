using Godot;

namespace MiniAbyss.Instances
{
    public class Player : Creature
    {
        public override void _Ready()
        {
            base._Ready();
            BattleGrid.Connect(nameof(Games.BattleGrid.EnemyTurnEndedSignal), this, nameof(OnEnemyTurnEnded));
        }

        public override void _Process(float delta)
        {
            CaptureInputs();
        }

        public override void OnActionFinished()
        {
            base.OnActionFinished();
            BattleGrid.EmitSignal(nameof(Games.BattleGrid.PlayerTurnEndedSignal));
        }

        private void OnEnemyTurnEnded()
        {
            SetProcess(true);
        }

        private void CaptureInputs()
        {
            if (Input.IsActionPressed("move_left")) HandleInput(Vector2.Left);
            else if (Input.IsActionPressed("move_right")) HandleInput(Vector2.Right);
            else if (Input.IsActionPressed("move_up")) HandleInput(Vector2.Up);
            else if (Input.IsActionPressed("move_down")) HandleInput(Vector2.Down);
        }

        private void HandleInput(Vector2 dir)
        {
            SetProcess(false);
            BattleGrid.HandleAction(this, dir);
        }
    }
}
