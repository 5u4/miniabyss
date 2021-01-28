using Godot;
using MiniAbyss.Instances;

namespace MiniAbyss.Items
{
    public class ItemData : Node
    {
        public string Display;
        public string Description;
        public SpriteFrames SpriteFrames;

        public virtual void Apply(Creature creature) {}

        public virtual void OnSell() {}
    }
}
