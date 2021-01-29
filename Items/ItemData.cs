using Godot;
using MiniAbyss.Instances;

namespace MiniAbyss.Items
{
    public class ItemData : Node
    {
        public int Level = 1;
        public int MaxLevel = 1;
        public string Display;
        public int Weight;
        public int Price;
        public int UpgradePrice;
        public SpriteFrames SpriteFrames;

        public virtual void Apply(Creature creature) {}

        public virtual string Description()
        {
            return "";
        }

        public virtual bool CanUpgrade()
        {
            return Level < MaxLevel;
        }

        public virtual void Upgrade()
        {
            Level++;
        }

        public virtual void OnSell() {}
    }
}
