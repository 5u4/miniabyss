using Godot;
using MiniAbyss.Data;

namespace MiniAbyss.Items
{
    public class HudItem : ItemData
    {
        public HudItem()
        {
            SpriteFrames = GD.Load<SpriteFrames>("res://SpriteFrames/HudSpriteFrames.tres");
        }

        public override void OnSell()
        {
            base.OnSell();
            PlayerData.Instance.HasHudItem = false;
        }
    }
}
