using Godot;
using MiniAbyss.Instances;

namespace MiniAbyss.Items
{
    public class Item : Control
    {
        public const string ItemScenePath = "res://Items/Item.tscn";

        public AnimatedSprite AnimatedSprite;
        public ItemData Data;

        public static Item MakeFromData(ItemData data)
        {
            var item = (Item) GD.Load<PackedScene>(ItemScenePath).Instance();
            item.Data = data;
            return item;
        }

        public override void _Ready()
        {
            AnimatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");

            AnimatedSprite.Frames = Data.SpriteFrames;
        }
    }
}
