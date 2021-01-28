using Godot;
using MiniAbyss.Data;

namespace MiniAbyss.Hud
{
    public class MoneyHud : Control
    {
        public AnimatedSprite Icon;
        public Label Amount;

        public override void _Ready()
        {
            Icon = GetNode<AnimatedSprite>("HBoxContainer/Icon/AnimatedSprite");
            Amount = GetNode<Label>("HBoxContainer/Amount/Label");

            PlayerData.Instance.Connect(nameof(PlayerData.MoneyUpdateSignal), this, nameof(OnMoneyUpdate));

            Init();
        }

        private void Init()
        {
            Amount.Text = PlayerData.Instance.Money.ToString();
        }

        private void OnMoneyUpdate(int amount)
        {
            Icon.Frame = 0;
            Icon.Play();
            Amount.Text = amount.ToString();
        }
    }
}
