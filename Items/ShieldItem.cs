using Godot;
using MiniAbyss.Instances;

namespace MiniAbyss.Items
{
    public class ShieldItem : ItemData
    {
        public const int UpgradeInc = 2;

        public int DefenceInc = 2;

        public ShieldItem()
        {
            MaxLevel = 5;
            Display = "Shield";
            Weight = 2;
            Price = 7;
            UpgradePrice = 12;
            SpriteFrames = GD.Load<SpriteFrames>("res://SpriteFrames/ShieldSpriteFrames.tres");
        }

        public override string Description()
        {
            return $"Increase your DEF by {DefenceInc}.";
        }

        public override void Apply(Creature creature)
        {
            base.Apply(creature);
            creature.Defence += DefenceInc;
        }

        public override void Upgrade()
        {
            base.Upgrade();
            DefenceInc += UpgradeInc;
        }
    }
}
