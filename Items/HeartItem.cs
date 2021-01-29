using Godot;
using MiniAbyss.Data;
using MiniAbyss.Instances;

namespace MiniAbyss.Items
{
    public class HeartItem : ItemData
    {
        public int HealthInc = 8;

        public HeartItem()
        {
            Display = "Heart";
            Description = $"Increase your HP by {HealthInc}.";
            SpriteFrames = GD.Load<SpriteFrames>("res://SpriteFrames/HeartSpriteFrames.tres");
        }

        public override void Apply(Creature creature)
        {
            base.Apply(creature);
            creature.Health += HealthInc;
            creature.MaxHealth += HealthInc;
            PlayerData.Instance.EmitSignal(nameof(PlayerData.HealthUpdateSignal), creature.Health, creature.MaxHealth);
        }
    }
}
