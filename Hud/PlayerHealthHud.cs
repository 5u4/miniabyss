using Godot;
using MiniAbyss.Data;

namespace MiniAbyss.Hud
{
    public class PlayerHealthHud : Control
    {
        public TextureProgress HealthBar;
        public AnimatedSprite HealthFrame;
        public Label CurrentHealth;
        public Label Slash;
        public Label MaxHealth;
        public Tween Tween;

        public override void _Ready()
        {
            HealthBar = GetNode<TextureProgress>("HBoxContainer/Bar/HealthBar");
            HealthFrame = GetNode<AnimatedSprite>("HBoxContainer/Bar/AnimatedSprite");
            CurrentHealth = GetNode<Label>("HBoxContainer/CurrentHealth/Label");
            Slash = GetNode<Label>("HBoxContainer/Slash/Label");
            MaxHealth = GetNode<Label>("HBoxContainer/MaxHealth/Label");
            Tween = GetNode<Tween>("Tween");

            PlayerData.Instance.Connect(nameof(PlayerData.HealthUpdateSignal), this, nameof(OnHealthUpdate));

            Init();
        }

        private void Init()
        {
            var h = PlayerData.Instance.Health;
            var mh = PlayerData.Instance.MaxHealth;
            HealthBar.Value = h;
            HealthBar.MaxValue = mh;
            CurrentHealth.Text = h.ToString();
            Slash.Text = "/";
            MaxHealth.Text = mh.ToString();
        }

        private void OnHealthUpdate(int health, int maxHealth)
        {
            HealthFrame.Frame = 0;
            HealthFrame.Play();
            HealthBar.MaxValue = maxHealth;
            Tween.InterpolateProperty(HealthBar, "value", HealthBar.Value, health, 0.3f,
                Tween.TransitionType.Back);
            const float intensity = 4f;
            var p1 = Vector2.Zero;
            var p2 = new Vector2((Rng.Instance.R.Randf() - 0.5f) * intensity, (Rng.Instance.R.Randf() - 0.5f) * intensity);
            var p3 = new Vector2((Rng.Instance.R.Randf() - 0.5f) * intensity, (Rng.Instance.R.Randf() - 0.5f) * intensity);
            Tween.InterpolateProperty(CurrentHealth, "rect_position", p1, p2, 0.2f, Tween.TransitionType.Back);
            Tween.InterpolateProperty(CurrentHealth, "rect_position", p2, p3, 0.2f, Tween.TransitionType.Back);
            Tween.InterpolateProperty(CurrentHealth, "rect_position", p3, p1, 0.2f, Tween.TransitionType.Back);
            Tween.Start();
            CurrentHealth.Text = health.ToString();
            MaxHealth.Text = maxHealth.ToString();
        }
    }
}
