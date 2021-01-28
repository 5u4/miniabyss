using Godot;
using MiniAbyss.Instances;

namespace MiniAbyss.Items
{
    public class Item : Control
    {
        public const string ItemScenePath = "res://Items/Item.tscn";

        public AnimatedSprite AnimatedSprite;

        public static Item MakeItem(string scriptPath)
        {
            var instance = GD.Load<PackedScene>(ItemScenePath).Instance();
            var s = GD.Load<Script>(scriptPath);
            var id = instance.GetInstanceId();
            instance.SetScript(s);
            return (Item) GD.InstanceFromId(id);
        }

        public virtual SpriteFrames GetSpriteFrames()
        {
            return null;
        }

        public override void _Ready()
        {
            AnimatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");

            AnimatedSprite.Frames = GetSpriteFrames();
        }

        public virtual void Apply(Creature creature) {}

        public virtual void OnSell() {}
    }
}
