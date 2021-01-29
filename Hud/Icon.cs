using Godot;
using MiniAbyss.Data;

namespace MiniAbyss.Hud
{
    public class Icon : Control
    {
        public AnimatedSprite AnimatedSprite;
        public Label Count;

        public override void _Ready()
        {
            AnimatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
            Count = GetNode<Label>("Count");

            Connect("mouse_entered", this, nameof(OnMouseEntered));
            Connect("mouse_exited", this, nameof(OnMouseExited));
        }

        public virtual void Rerender() {}

        public virtual string MakeDescription()
        {
            return "";
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
