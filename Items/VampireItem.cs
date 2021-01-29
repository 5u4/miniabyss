using Godot;
using MiniAbyss.Instances;

namespace MiniAbyss.Items
{
    public class VampireItem : ItemData
    {
        public float Ratio = 0.2f;

        public VampireItem()
        {
            Display = "Vampire";
            Description = $"Heal {Ratio * 100}% of the damage you dealt.";
            SpriteFrames = GD.Load<SpriteFrames>("res://SpriteFrames/VampireSpriteFrames.tres");
        }

        public override void Apply(Creature creature)
        {
            base.Apply(creature);
            creature.AfterDamages.Add((dealer, _, amount) => dealer.Heal(Mathf.CeilToInt(amount * Ratio)));
        }
    }
}
