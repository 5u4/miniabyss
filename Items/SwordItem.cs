using Godot;
using MiniAbyss.Instances;

namespace MiniAbyss.Items
{
    public class SwordItem : ItemData
    {
        public const int UpgradeInc = 2;

        public int StrengthInc = 3;

        public SwordItem()
        {
            MaxLevel = 5;
            Display = "Sword";
            Weight = 4;
            Price = 7;
            UpgradePrice = 12;
            SpriteFrames = GD.Load<SpriteFrames>("res://SpriteFrames/SwordSpriteFrames.tres");
        }

        public override string Description()
        {
            return $"Increase your ATK by {StrengthInc}.";
        }

        public override void Apply(Creature creature)
        {
            base.Apply(creature);
            creature.Strength += StrengthInc;
        }

        public override void Upgrade()
        {
            base.Upgrade();
            StrengthInc += UpgradeInc;
        }
    }
}
