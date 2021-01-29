using System.Collections.Generic;
using Godot;
using MiniAbyss.Instances;
using MiniAbyss.StatusEffects;

namespace MiniAbyss.Hud
{
    public class StatusContainer : GridContainer
    {
        public Creature Creature;
        public Dictionary<string, Icon> StatusIcons;

        public override void _Ready()
        {
            StatusIcons = new Dictionary<string, Icon>();
        }

        public void UpdateIcons()
        {
            foreach (var pair in Creature.StatusManager.Status)
            {
                if (!StatusIcons.ContainsKey(pair.Key)) AddStatusIcon(pair.Value);
                else StatusIcons[pair.Key].Rerender();
            }
        }

        private void AddStatusIcon(StatusHandler handler)
        {
            var icon = handler.MakeIcon();
            StatusIcons[handler.GetDisplay()] = icon;
            AddChild(icon);
            icon.Rerender();
        }
    }
}
