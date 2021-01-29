using Godot;
using MiniAbyss.Data;

namespace MiniAbyss.Items
{
    public class HudItem : ItemData
    {
        public HudItem()
        {
            Display = "HUD";
            Description = "Used for displaying UI elements.";
            Weight = 2;
            Price = 8;
            SpriteFrames = GD.Load<SpriteFrames>("res://SpriteFrames/HudSpriteFrames.tres");
        }

        public override void OnSell()
        {
            base.OnSell();
            PlayerData.Instance.HasHudItem = false;
        }
    }
}
