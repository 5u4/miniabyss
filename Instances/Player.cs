using Godot;
using MiniAbyss.Data;

namespace MiniAbyss.Instances
{
    public class Player : Creature
    {
        public const int InitialHealth = 5;

        public override void _Ready()
        {
            base._Ready();
            InitializePlayer();

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

        public void InitializePlayer()
        {
            MaxHealth = InitialHealth;
            Health = InitialHealth;
        }

        public override void OnDeath()
        {
            base.OnDeath();
            // TODO: Handle game over
        }

        private void OnEnemyTurnEnded()
        {
            SetProcess(true);
        }

        public override void Heal(int amount)
        {
            base.Heal(amount);
            PlayerData.Instance.EmitSignal(nameof(PlayerData.HealthUpdateSignal), Health, MaxHealth);
        }

        public override int Hit(int amount, Creature dealer, bool pure = false)
        {
            var finalAmount = base.Hit(amount, dealer, pure);
            PlayerData.Instance.EmitSignal(nameof(PlayerData.HealthUpdateSignal), Health, MaxHealth);
            return finalAmount;
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
