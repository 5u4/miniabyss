using System.Collections.Generic;
using MiniAbyss.Instances;

namespace MiniAbyss.StatusEffects
{
    public class StatusManager
    {
        public Creature Creature;
        public Dictionary<string, StatusHandler> Status;

        public StatusManager(Creature creature)
        {
            Creature = creature;
            Status = new Dictionary<string, StatusHandler>();
        }

        public void Tick()
        {
            foreach (var s in Status.Values) s.Tick(Creature);
        }

        public void Add(StatusHandler s)
        {
            var display = s.GetDisplay();
            if (Status.ContainsKey(display)) Status[display].Extend(s);
            else Status[display] = s;
        }
    }
}
