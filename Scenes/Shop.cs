using System.Collections.Generic;
using Godot;
using MiniAbyss.Data;
using MiniAbyss.Hud;
using MiniAbyss.Items;

namespace MiniAbyss.Scenes
{
    public class Shop : Node2D
    {
        [Export] public PackedScene ItemSelectScene;

        public Control LootPickerContainer;
        public Transition Transition;
        public InventoryIndicator InventoryIndicator;
        public Button NextButton;
        public Button SellButton;
        public Button UpgradeButton;
        public List<ItemSelect> Inventory;

        public int InventorySize;
        public int InventoryCapacity;
        public ItemSelect CurrentSelect;

        public override void _Ready()
        {
            LootPickerContainer = GetNode<Control>("CanvasLayer/VBoxContainer/LootPickerContainer");
            Transition = GetNode<Transition>("TransitionLayer/Transition");
            NextButton = GetNode<Button>("CanvasLayer/VBoxContainer/NextButton/NextButton");
            SellButton = GetNode<Button>("CanvasLayer/VBoxContainer/SellUpgradeGroup/Sell/Sell");
            UpgradeButton = GetNode<Button>("CanvasLayer/VBoxContainer/SellUpgradeGroup/Upgrade/Upgrade");
            InventoryIndicator = GetNode<InventoryIndicator>("CanvasLayer/VBoxContainer/InventoryIndicator");

            InventorySize = PlayerData.Instance.GetInventorySize();
            InventoryCapacity = PlayerData.Instance.GetInventoryCapacity();
            InventoryIndicator.OnChangeInventory(InventorySize, 0, InventoryCapacity);

            StartTransition();

            UpdateSellButton();
            UpdateUpgradeButton();

            NextButton.Connect("pressed", this, nameof(OnNextButtonPressed));
            SellButton.Connect("pressed", this, nameof(OnSellButtonPressed));
            UpgradeButton.Connect("pressed", this, nameof(OnUpgradeButtonPressed));

            Inventory = GenerateInventory();
            Inventory.ForEach(item => LootPickerContainer.AddChild(item));
        }

        private async void StartTransition()
        {
            Transition.Start();
            await ToSignal(Transition.Tween, "tween_all_completed");
            Transition.Hide();
        }

        private async void OnNextButtonPressed()
        {
            Transition.Close();
            await ToSignal(Transition.Tween, "tween_all_completed");
            GetTree().ChangeScene("res://Scenes/Level.tscn");
        }

        private void OnSellButtonPressed()
        {
            if (CurrentSelect == null) return;
            CurrentSelect.Item.Data.OnSell();
            var newMoney = PlayerData.Instance.Money + CurrentSelect.Item.Data.Price;
            PlayerData.Instance.EmitSignal(nameof(PlayerData.MoneyUpdateSignal), newMoney);
            InventorySize -= CurrentSelect.Item.Data.Weight;
            InventoryIndicator.OnChangeInventory(InventorySize, 0, InventoryCapacity);
            var parent = CurrentSelect.GetParent();
            parent.RemoveChild(CurrentSelect);
            PlayerData.Instance.Inventory.Remove(CurrentSelect.Item.Data);
            CurrentSelect = null;
            UpdateSellButton();
        }

        private void OnUpgradeButtonPressed()
        {
            // TODO
            UpdateUpgradeButton();
        }

        private void UpdateSellButton()
        {
            if (CurrentSelect == null)
            {
                SellButton.Disabled = true;
                SellButton.Text = "Sell";
                return;
            }

            SellButton.Text = $"Sell for ${CurrentSelect.Item.Data.Price}";
            SellButton.Disabled = false;
        }

        private void UpdateUpgradeButton()
        {
            if (CurrentSelect == null)
            {
                UpgradeButton.Disabled = true;
                UpgradeButton.Text = "Upgrade";
                return;
            }
            var canUpgrade = CurrentSelect.Item.Data.CanUpgrade();
            UpgradeButton.Text = canUpgrade
                ? $"Upgrade ${CurrentSelect.Item.Data.UpgradePrice}"
                : "Cannot Upgrade";
            UpgradeButton.Disabled = !canUpgrade;
        }

        private List<ItemSelect> GenerateInventory()
        {
            var inventory = new List<ItemSelect>();
            PlayerData.Instance.Inventory.ForEach(data => inventory.Add(MakeOneItem(data)));
            return inventory;
        }

        private ItemSelect MakeOneItem(ItemData data)
        {
            var item = Item.MakeFromData(data);
            var select = (ItemSelect) ItemSelectScene.Instance();
            select.Item = item;
            select.OnSelect = selected =>
            {
                CurrentSelect?.Toggle();
                CurrentSelect = select;
                var selectedWeight = CurrentSelect.Item.Data.Weight;
                var size = InventorySize - selectedWeight;
                InventoryIndicator.OnChangeInventory(size, selectedWeight, InventoryCapacity);
                UpdateSellButton();
                UpdateUpgradeButton();
            };
            select.OnUnSelect = selected =>
            {
                CurrentSelect = null;
                InventoryIndicator.OnChangeInventory(InventorySize, 0, InventoryCapacity);
                UpdateSellButton();
                UpdateUpgradeButton();
            };
            return select;
        }
    }
}
