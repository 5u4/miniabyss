using Godot;
using MiniAbyss.Hud;

namespace MiniAbyss.Items
{
    public class Item : Icon
    {
        public const string ItemScenePath = "res://Items/Item.tscn";

        public ItemData Data;

        public static Item MakeFromData(ItemData data)
        {
            var item = (Item) GD.Load<PackedScene>(ItemScenePath).Instance();
            item.Data = data;
            return item;
        }

        public override void _Ready()
        {
            base._Ready();
            AnimatedSprite.Frames = Data.SpriteFrames;
        }

        public override string MakeDescription()
        {
            return $"[{Data.Display}] {Data.Description}";
        }
    }
}
