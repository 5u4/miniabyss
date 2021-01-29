using Godot;
using MiniAbyss.Data;

namespace MiniAbyss.Scenes
{
    public class DepthDisplay : Node2D
    {
        [Export] public PackedScene NextScene;

        public const int DepthRange = 3;
        public const int DepthBase = 4;

        public Label Depth;
        public Tween Tween;

        public override void _Ready()
        {
            Depth = GetNode<Label>("CanvasLayer/Depth");
            Tween = GetNode<Tween>("Tween");

            UpdateDepth();

            Depth.Text = $"- Depth: {PlayerData.Instance.Depth} -";

            Depth.Modulate = Colors.Transparent;
            Transit();
        }

        private void UpdateDepth()
        {
            PlayerData.Instance.LastDepth = PlayerData.Instance.Depth;
            var depthInc = Mathf.RoundToInt((Rng.Instance.G.Randf() - 0.5f) * DepthRange + DepthBase);
            PlayerData.Instance.Depth += depthInc;
        }

        private async void Transit()
        {
            Tween.InterpolateProperty(Depth, "modulate", Colors.Transparent, Colors.White, 1);
            Tween.InterpolateProperty(Depth, "modulate", Colors.White, Colors.Transparent, 1,
                Tween.TransitionType.Linear, Tween.EaseType.InOut, 4);
            Tween.Start();
            await ToSignal(Tween, "tween_all_completed");
            GetTree().ChangeSceneTo(NextScene);
        }
    }
}
