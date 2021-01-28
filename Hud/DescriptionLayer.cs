using Godot;
using MiniAbyss.Data;

namespace MiniAbyss.Hud
{
    public class DescriptionLayer : CanvasLayer
    {
        public Label DescriptionLabel;

        public override void _Ready()
        {
            DescriptionLabel = GetNode<Label>("CenterContainer/Control/Description");

            PlayerData.Instance.Connect(nameof(PlayerData.ShowDescriptionSignal), this, nameof(OnShowDescription));
            PlayerData.Instance.Connect(nameof(PlayerData.HideDescriptionSignal), this, nameof(OnHideDescription));
        }

        private void OnShowDescription(string description)
        {
            DescriptionLabel.Text = description;
            DescriptionLabel.Show();
        }

        private void OnHideDescription()
        {
            DescriptionLabel.Hide();
        }
    }
}
