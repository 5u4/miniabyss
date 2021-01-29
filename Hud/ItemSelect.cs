using Godot;
using MiniAbyss.Data;
using MiniAbyss.Items;

namespace MiniAbyss.Hud
{
    public class ItemSelect : Control
    {
        public delegate bool CanSelectFn(Item item);

        public delegate void OnSelectFn(Item item);

        public delegate void OnUnSelectFn(Item item);

        public AnimatedSprite AnimatedSprite;
        public bool Selected;
        public Control Holder;
        public Item Item;
        public Tween Tween;
        public CanSelectFn CanSelect = _ => true;
        public OnSelectFn OnSelect = _ => {};
        public OnUnSelectFn OnUnSelect = _ => {};

        public override void _Ready()
        {
            Holder = GetNode<Control>("Holder");
            AnimatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
            Tween = GetNode<Tween>("Tween");

            Holder.AddChild(Item);

            Connect("pressed", this, nameof(OnPressed));
        }

        public void Toggle()
        {
            if (!Selected && !CanSelect.Invoke(Item))
            {
                Shake();
                return;
            }
            AnimatedSprite.Play(Selected ? "unselect" : "select");
            Selected = !Selected;

            if (Selected) OnSelect.Invoke(Item);
            else OnUnSelect.Invoke(Item);
        }

        private void OnPressed()
        {
            Toggle();
        }

        private void Shake()
        {
            const float intensity = 8f;
            const float duration = 0.2f;
            var p1 = Vector2.Zero;
            var p2 = new Vector2((Rng.Instance.R.Randf() - 0.5f) * intensity, (Rng.Instance.R.Randf() - 0.5f) * intensity);
            var p3 = new Vector2((Rng.Instance.R.Randf() - 0.5f) * intensity, (Rng.Instance.R.Randf() - 0.5f) * intensity);
            Tween.InterpolateProperty(Holder, "rect_position", p1, p2, duration, Tween.TransitionType.Back);
            Tween.InterpolateProperty(Holder, "rect_position", p2, p3, duration, Tween.TransitionType.Back,
                Tween.EaseType.InOut, duration);
            Tween.InterpolateProperty(Holder, "rect_position", p3, p1, duration, Tween.TransitionType.Back,
                Tween.EaseType.InOut, duration);
            Tween.Start();
        }
    }
}
