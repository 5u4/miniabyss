using Godot;
using MiniAbyss.Instances;

namespace MiniAbyss.Hud
{
    public class DamagePopText : Control
    {
        public Tween Tween;
        public Label Label;

        public override void _Ready()
        {
            Tween = GetNode<Tween>("Tween");
            Label = GetNode<Label>("Label");
        }

        public async void Pop(int amount, Vector2 at)
        {
            Label.Text = Mathf.Abs(amount).ToString();
            var spriteOffset = new Vector2(Entity.SpriteDimension, Entity.SpriteDimension) / 2;
            at -= Label.RectSize / 2 - spriteOffset;
            Label.RectGlobalPosition = at;
            if (amount != 0)
                Label.AddColorOverride("font_color",
                    amount < 0 ? new Color(1f, 0.33f, 0.33f) : new Color(0.33f, 1f, 0.33f));
            var initialVal = at;
            var finalVal = at + new Vector2(GD.Randf() * 2 - 1, -8);
            Tween.InterpolateProperty(Label, "rect_global_position", initialVal, finalVal, 1f,
                Tween.TransitionType.Back, Tween.EaseType.Out);
            Tween.Start();
            await ToSignal(Tween, "tween_all_completed");
            QueueFree();
        }
    }
}
