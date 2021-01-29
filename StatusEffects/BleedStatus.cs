using System;
using Godot;
using MiniAbyss.Hud;
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
            if (CanRemove()) return;
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

        public override Icon MakeIcon()
        {
            var i = (BleedIcon) GD.Load<PackedScene>("res://StatusEffects/BleedIcon.tscn").Instance();
            i.Handler = this;
            return i;
        }

        public override bool CanRemove()
        {
            return TotalDamage <= 0;
        }
    }
}
