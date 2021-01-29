using System.Collections.Generic;
using System.Linq;
using Godot;
using MiniAbyss.Instances;
using MiniAbyss.Items;

namespace MiniAbyss.Data
{
    /**
     * More like a game manager
     */
    public class PlayerData : Node
    {
        [Signal]
        public delegate void HealthUpdateSignal(int health, int maxHealth);

        [Signal]
        public delegate void MoneyUpdateSignal(int amount);

        [Signal]
        public delegate void ShowDescriptionSignal(string description);

        [Signal]
        public delegate void HideDescriptionSignal();

        public const int BaseCapacity = 10;

        private PlayerData()
        {
            Inventory = new List<ItemData> {new HudItem()};
        }

        public static PlayerData Instance { get; } = new PlayerData();

        public int MaxHealth { get; private set; } = Player.InitialHealth;
        public int Health { get; private set; } = Player.InitialHealth;
        public int Money;
        public List<ItemData> Inventory;
        public bool HasHudItem = true;
        public int Kills = 0;
        public int LastDepth = 1;
        public int Depth = 1;

        public override void _Ready()
        {
            Instance.Connect(nameof(HealthUpdateSignal), this, nameof(OnHealthUpdate));
            Instance.Connect(nameof(MoneyUpdateSignal), this, nameof(OnMoneyUpdate));
        }

        public float GetItemQuality()
        {
            // TODO
            return 5;
        }

        public int GetInventorySize()
        {
            return Instance.Inventory.Sum(item => item.Weight);
        }

        public int GetInventoryCapacity()
        {
            // TODO
            return BaseCapacity;
        }

        private void OnHealthUpdate(int health, int maxHealth)
        {
            Instance.Health = health;
            Instance.MaxHealth = maxHealth;
        }

        private void OnMoneyUpdate(int amount)
        {
            Instance.Money = amount;
        }
    }
}
