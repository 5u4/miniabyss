using Godot;
using MiniAbyss.Instances;

namespace MiniAbyss.Items
{
    public class ShieldItem : ItemData
    {
        public int DefenceInc = 2;

        public ShieldItem()
        {
            Display = "Shield";
            Description = $"Increase your DEF by {DefenceInc}.";
            Weight = 4;
            Price = 7;
            SpriteFrames = GD.Load<SpriteFrames>("res://SpriteFrames/ShieldSpriteFrames.tres");
        }

        public override void Apply(Creature creature)
        {
            base.Apply(creature);
            creature.Defence += DefenceInc;
        }
    }
}
