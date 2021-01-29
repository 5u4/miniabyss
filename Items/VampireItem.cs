using Godot;
using MiniAbyss.Instances;

namespace MiniAbyss.Items
{
    public class VampireItem : ItemData
    {
        public const float UpgradeInc = 0.15f;

        public float Ratio = 0.2f;

        public VampireItem()
        {
            MaxLevel = 4;
            Display = "Vampire";
            Weight = 6;
            Price = 12;
            SpriteFrames = GD.Load<SpriteFrames>("res://SpriteFrames/VampireSpriteFrames.tres");
        }

        public override string Description()
        {
            return $"Heal {Ratio * 100}% of the damage you dealt.";
        }

        public override void Apply(Creature creature)
        {
            base.Apply(creature);
            creature.AfterDamages.Add((dealer, _, amount) => dealer.Heal(Mathf.CeilToInt(amount * Ratio)));
        }

        public override void Upgrade()
        {
            base.Upgrade();
            Ratio += UpgradeInc;
        }
    }
}
