using Godot;
using MiniAbyss.Data;

namespace MiniAbyss.Hud
{
    public class DescriptionArea : Area2D
    {
        [Export] public string Description;
        [Export] public float ExtendDimension = 4;

        public delegate void BeforeShowHook();

        public BeforeShowHook BeforeShow;
        public CollisionShape2D CollisionShape2D;

        public override void _Ready()
        {
            CollisionShape2D = GetNode<CollisionShape2D>("CollisionShape2D");

            CollisionShape2D.Shape = new RectangleShape2D {Extents = new Vector2(ExtendDimension, ExtendDimension)};
            CollisionShape2D.Position = new Vector2(ExtendDimension, ExtendDimension);

            Connect("mouse_entered", this, nameof(OnMouseEntered));
            Connect("mouse_exited", this, nameof(OnMouseExited));
        }

        private void OnMouseEntered()
        {
            BeforeShow?.Invoke();
            PlayerData.Instance.EmitSignal(nameof(PlayerData.ShowDescriptionSignal), Description);
        }

        private void OnMouseExited()
        {
            PlayerData.Instance.EmitSignal(nameof(PlayerData.HideDescriptionSignal));
        }
    }
}
