using Godot;
using MiniAbyss.Items;

namespace MiniAbyss.Hud
{
    public class ItemSelect : Control
    {
        public AnimatedSprite AnimatedSprite;
        public bool Selected;
        public Item Item;

        public override void _Ready()
        {
            AnimatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");

            Connect("pressed", this, nameof(OnPressed));
        }

        private void OnPressed()
        {
            AnimatedSprite.Play(Selected ? "unselect" : "select");
            Selected = !Selected;
        }
    }
}
