using Godot;
using MiniAbyss.Data;
using MiniAbyss.Instances;

namespace MiniAbyss.Items
{
    public class HeartItem : ItemData
    {
        public const int UpgradeInc = 6;

        public int HealthInc = 8;

        public HeartItem()
        {
            MaxLevel = 5;
            Display = "Heart";
            Weight = 3;
            Price = 10;
            UpgradePrice = 15;
            SpriteFrames = GD.Load<SpriteFrames>("res://SpriteFrames/HeartSpriteFrames.tres");
        }

        public override string Description()
        {
            return $"Increase your HP by {HealthInc}.";
        }

        public override void Apply(Creature creature)
        {
            base.Apply(creature);
            creature.Health += HealthInc;
            creature.MaxHealth += HealthInc;
            PlayerData.Instance.EmitSignal(nameof(PlayerData.HealthUpdateSignal), creature.Health, creature.MaxHealth);
        }

        public override void Upgrade()
        {
            base.Upgrade();
            HealthInc += UpgradeInc;
        }
    }
}
