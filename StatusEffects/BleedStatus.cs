using System;
using Godot;
using MiniAbyss.Instances;

namespace MiniAbyss.StatusEffects
{
    public class BleedStatus : StatusHandler
    {
        public const int MaxTurn = 4;

        public int TotalDamage;

        public override string GetDisplay()
        {
            return "Bleed";
        }

        public override void Tick(Creature creature)
        {
            if (TotalDamage <= 0) return;
            var dmg = Mathf.Min(Mathf.CeilToInt((float) TotalDamage / MaxTurn), TotalDamage);
            TotalDamage -= dmg;
            creature.Hit(dmg, null, true);
        }

        public override void Extend(StatusHandler other)
        {
            if (!(other is BleedStatus bleed)) throw new Exception("Cannot extend different kind of status");
            TotalDamage += bleed.TotalDamage;
            Turn += bleed.Turn;
        }
    }
}
