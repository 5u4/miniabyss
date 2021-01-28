using System;
using System.Collections.Generic;
using Godot;
using MiniAbyss.Items;

namespace MiniAbyss.Data
{
    public class PlayerData : Node
    {
        [Signal]
        public delegate void HealthUpdateSignal(int health, int maxHealth);

        [Signal]
        public delegate void MoneyUpdateSignal(int amount);

        private PlayerData() {}

        public static PlayerData Instance { get; } = new PlayerData();

        public int MaxHealth = 5;
        public int Health = 5;
        public int Money;
        public readonly List<Func<Item>> Inventory = new List<Func<Item>> {HudItem.Make};
        public bool HasHudItem = true;

        public override void _Ready()
        {
            Connect(nameof(HealthUpdateSignal), this, nameof(OnHealthUpdate));
            Connect(nameof(MoneyUpdateSignal), this, nameof(OnMoneyUpdate));
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
