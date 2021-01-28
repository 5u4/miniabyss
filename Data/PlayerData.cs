using Godot;

namespace MiniAbyss.Data
{
    public class PlayerData : Node
    {
        [Signal]
        public delegate void HealthUpdateSignal(int health, int maxHealth);

        private PlayerData() {}

        public static PlayerData Instance { get; } = new PlayerData();

        public int MaxHealth = 5;
        public int Health = 5;
        public int Money;

        public override void _Ready()
        {
            Connect(nameof(HealthUpdateSignal), this, nameof(OnHealthUpdate));
        }

        private void OnHealthUpdate(int health, int maxHealth)
        {
            Instance.Health = health;
            Instance.MaxHealth = maxHealth;
        }
    }
}
