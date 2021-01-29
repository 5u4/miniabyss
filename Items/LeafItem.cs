using Godot;
using MiniAbyss.Instances;

namespace MiniAbyss.Items
{
    public class LeafItem : ItemData
    {
        public const float UpgradeInc = 0.2f;

        public float HealEffInc = 0.3f;

        public LeafItem()
        {
            MaxLevel = 4;
            Display = "Leaf";
            Weight = 4;
            Price = 7;
            UpgradePrice = 12;
            SpriteFrames = GD.Load<SpriteFrames>("res://SpriteFrames/LeafSpriteFrames.tres");
        }

        public override string Description()
        {
            return $"Increase all heals by {HealEffInc * 100}%.";
        }

        public override void Apply(Creature creature)
        {
            base.Apply(creature);
            creature.HealEfficiency += HealEffInc;
        }

        public override void Upgrade()
        {
            base.Upgrade();
            HealEffInc += UpgradeInc;
        }
    }
}
