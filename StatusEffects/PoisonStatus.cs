using System;
using Godot;
using MiniAbyss.Hud;
using MiniAbyss.Instances;

namespace MiniAbyss.StatusEffects
{
    public class PoisonStatus : StatusHandler
    {
        public override string GetDisplay()
        {
            return "Poison";
        }

        public override void Tick(Creature creature)
        {
            if (CanRemove()) return;
            creature.Hit(Turn, null, true);
            Turn--;
        }

        public override void Extend(StatusHandler other)
        {
            if (!(other is PoisonStatus poison)) throw new Exception("Cannot extend different kind of status");
            Turn += poison.Turn;
        }

        public override Icon MakeIcon()
        {
            var i = (PoisonIcon) GD.Load<PackedScene>("res://StatusEffects/PoisonIcon.tscn").Instance();
            i.Handler = this;
            return i;
        }

        public override bool CanRemove()
        {
            return Turn <= 0;
        }
    }
}
