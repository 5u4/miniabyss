using Godot;
using MiniAbyss.Data;
using MiniAbyss.Hud;
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

            Connect("mouse_entered", this, nameof(OnMouseEntered));
            Connect("mouse_exited", this, nameof(OnMouseExited));
        }

        public string MakeDescription()
        {
            return $"[{Data.Display}] {Data.Description}";
        }

        private void OnMouseEntered()
        {
            AnimatedSprite.Frame = 0;
            AnimatedSprite.Play();
            PlayerData.Instance.EmitSignal(nameof(PlayerData.ShowDescriptionSignal), MakeDescription());
        }

        private void OnMouseExited()
        {
            if (MouseOverAnyArea()) return;
            PlayerData.Instance.EmitSignal(nameof(PlayerData.HideDescriptionSignal));
        }

        private bool MouseOverAnyArea()
        {
            var state = GetWorld2d().DirectSpaceState;
            var res = state.IntersectPoint(GetGlobalMousePosition(), 1, null, uint.MaxValue, false, true);
            return res.Count > 0;
        }
    }
}
