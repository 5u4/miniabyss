using Godot;
using MiniAbyss.Instances;

namespace MiniAbyss.Items
{
    public class ItemData : Node
    {
        public string Display;
        public string Description;
        public int Weight;
        public int Price;
        public int UpgradePrice;
        public SpriteFrames SpriteFrames;

        public virtual void Apply(Creature creature) {}

        public virtual bool CanUpgrade()
        {
            return false;
        }

        public virtual void Upgrade() {}

        public virtual void OnSell() {}
    }
}
