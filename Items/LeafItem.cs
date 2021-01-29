using Godot;
using MiniAbyss.Instances;

namespace MiniAbyss.Items
{
    public class LeafItem : ItemData
    {
        public float HealEffInc = 0.3f;

        public LeafItem()
        {
            Display = "Leaf";
            Description = $"Increase all heals by {Mathf.CeilToInt(HealEffInc)}%.";
            SpriteFrames = GD.Load<SpriteFrames>("res://SpriteFrames/LeafSpriteFrames.tres");
        }

        public override void Apply(Creature creature)
        {
            base.Apply(creature);
            creature.HealEfficiency += HealEffInc;
        }
    }
}
