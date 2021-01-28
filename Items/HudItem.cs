using Godot;
using MiniAbyss.Data;

namespace MiniAbyss.Items
{
    public class HudItem : Item
    {
        public static Item Make()
        {
            return MakeItem("res://Items/HudItem.cs");
        }

        public override SpriteFrames GetSpriteFrames()
        {
            return GD.Load<SpriteFrames>("res://SpriteFrames/HudSpriteFrames.tres");
        }

        public override void OnSell()
        {
            base.OnSell();
            PlayerData.Instance.HasHudItem = false;
        }
    }
}
