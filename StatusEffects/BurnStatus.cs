using System;
using Godot;
using MiniAbyss.Hud;
using MiniAbyss.Instances;

namespace MiniAbyss.StatusEffects
{
    public class BurnStatus : StatusHandler
    {
        public const float DamageRatio = 0.05f;

        public override string GetDisplay()
        {
            return "Burn";
        }

        public override void Tick(Creature creature)
        {
            if (CanRemove()) return;
            var dmg = Mathf.CeilToInt(creature.MaxHealth * DamageRatio);
            creature.Hit(dmg, null, true);
            Turn--;
        }

        public override void Extend(StatusHandler other)
        {
            if (!(other is BurnStatus burn)) throw new Exception("Cannot extend different kind of status");
            Turn += burn.Turn;
        }

        public override Icon MakeIcon()
        {
            var i = (BurnIcon) GD.Load<PackedScene>("res://StatusEffects/BurnIcon.tscn").Instance();
            i.Handler = this;
            return i;
        }

        public override bool CanRemove()
        {
            return Turn <= 0;
        }
    }
}
