using Godot;
using MiniAbyss.Instances;

namespace MiniAbyss.Items
{
    public class SwordItem : ItemData
    {
        public int StrengthInc = 3;

        public SwordItem()
        {
            Display = "Sword";
            Description = $"Increase your ATK by {StrengthInc}.";
            Weight = 4;
            Price = 7;
            SpriteFrames = GD.Load<SpriteFrames>("res://SpriteFrames/SwordSpriteFrames.tres");
        }

        public override void Apply(Creature creature)
        {
            base.Apply(creature);
            creature.Strength += StrengthInc;
        }
    }
}
