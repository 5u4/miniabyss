using Godot;
using MiniAbyss.Data;

namespace MiniAbyss.Hud
{
    public class InventoryIndicator : Control
    {
        public ColorRect Progress;
        public Label Label;
        public Tween Tween;

        public override void _Ready()
        {
            Progress = GetNode<ColorRect>("VBoxContainer/HBoxContainer/Progress");
            Label = GetNode<Label>("VBoxContainer/HBoxContainer/Control/Label");
            Tween = GetNode<Tween>("Tween");
        }

        public void Shake()
        {
            const float intensity = 8f;
            const float duration = 0.2f;
            var p1 = Vector2.Zero;
            var p2 = new Vector2((Rng.Instance.R.Randf() - 0.5f) * intensity, (Rng.Instance.R.Randf() - 0.5f) * intensity);
            var p3 = new Vector2((Rng.Instance.R.Randf() - 0.5f) * intensity, (Rng.Instance.R.Randf() - 0.5f) * intensity);
            Tween.InterpolateProperty(Label, "rect_position", p1, p2, duration, Tween.TransitionType.Back);
            Tween.InterpolateProperty(Label, "rect_position", p2, p3, duration, Tween.TransitionType.Back,
                Tween.EaseType.InOut, duration);
            Tween.InterpolateProperty(Label, "rect_position", p3, p1, duration, Tween.TransitionType.Back,
                Tween.EaseType.InOut, duration);
            Tween.Start();
        }

        public void OnChangeInventory(int size, int predSize, int capacity)
        {
            Label.Text = $"{size + predSize}/{capacity}";
            Progress.Material.Set("shader_param/size", size);
            Progress.Material.Set("shader_param/pred_size", predSize);
            Progress.Material.Set("shader_param/capacity", capacity);
        }
    }
}
