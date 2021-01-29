using Godot;

namespace MiniAbyss.Hud
{
    public class Transition : ColorRect
    {
        public Tween Tween;

        public override void _Ready()
        {
            Tween = GetNode<Tween>("Tween");
        }

        public void Start(float from = 0, float to = 1, float duration = 1)
        {
            Material.Set("shader_param/cutoff", from);
            Show();
            Tween.InterpolateProperty(Material, "shader_param/cutoff", from, to, duration);
            Tween.Start();
        }

        public void Close(float duration = 1f)
        {
            Start(1, 0, duration);
        }
    }
}
