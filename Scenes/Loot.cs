using System.Collections.Generic;
using Godot;
using MiniAbyss.Data;
using MiniAbyss.Hud;
using MiniAbyss.Items;

namespace MiniAbyss.Scenes
{
    public class Loot : Node2D
    {
        public delegate ItemData MakeItemData();

        [Export] public PackedScene ItemSelectScene;
        [Export] public PackedScene NextScene;

        public Control LootPickerContainer;
        public Transition Transition;
        public InventoryIndicator InventoryIndicator;
        public Button NextButton;
        public List<ItemSelect> Loots;

        public List<MakeItemData> ItemDataMakers;

        public int InventorySize;
        public int InventoryPredSize;
        public int InventoryCapacity;

        public override void _Ready()
        {
            ItemDataMakers = new List<MakeItemData>
            {
                () => new ShieldItem(),
                () => new HeartItem(),
                () => new SwordItem(),
                () => new LeafItem(),
                () => new VampireItem(),
            };

            LootPickerContainer = GetNode<Control>("CanvasLayer/VBoxContainer/LootPickerContainer");
            Transition = GetNode<Transition>("TransitionLayer/Transition");
            NextButton = GetNode<Button>("CanvasLayer/VBoxContainer/Control/NextButton");
            InventoryIndicator = GetNode<InventoryIndicator>("CanvasLayer/VBoxContainer/InventoryIndicator");

            InventorySize = PlayerData.Instance.GetInventorySize();
            InventoryPredSize = 0;
            InventoryCapacity = PlayerData.Instance.GetInventoryCapacity();
            InventoryIndicator.OnChangeInventory(InventorySize, InventoryPredSize, InventoryCapacity);

            StartTransition();

            NextButton.Connect("pressed", this, nameof(OnNextButtonPressed));

            Loots = GenerateLoots();
            Loots.ForEach(loot => LootPickerContainer.AddChild(loot));
        }

        private async void StartTransition()
        {
            Transition.Start();
            await ToSignal(Transition.Tween, "tween_all_completed");
            Transition.Hide();
        }

        private async void OnNextButtonPressed()
        {
            CollectLoots();
            Transition.Close();
            await ToSignal(Transition.Tween, "tween_all_completed");
            GetTree().ChangeScene("res://Scenes/Level.tscn");
        }

        private void CollectLoots()
        {
            Loots.ForEach(loot =>
            {
                if (!loot.Selected) return;
                PlayerData.Instance.Inventory.Add(loot.Item.Data);
            });
        }

        private List<ItemSelect> GenerateLoots()
        {
            var loots = new List<ItemSelect>();
            var quality = PlayerData.Instance.GetItemQuality();
            var quantity = PlayerData.Instance.Kills + PlayerData.Instance.LastDepth;
            for (var i = 0; i < quantity; i++) loots.Add(MakeOneLoot(quality));
            return loots;
        }

        private ItemSelect MakeOneLoot(float quality)
        {
            var i = Mathf.FloorToInt(Rng.Instance.G.Randf() * Mathf.Min(ItemDataMakers.Count, quality));
            var itemData = ItemDataMakers[i].Invoke();
            var item = Item.MakeFromData(itemData);
            var select = (ItemSelect) ItemSelectScene.Instance();
            select.Item = item;
            select.CanSelect = selected =>
            {
                var canSelect = selected.Data.Weight + InventorySize + InventoryPredSize <= InventoryCapacity;
                if (!canSelect) InventoryIndicator.Shake();
                return canSelect;
            };
            select.OnSelect = selected =>
            {
                InventoryPredSize += selected.Data.Weight;
                InventoryIndicator.OnChangeInventory(InventorySize, InventoryPredSize, InventoryCapacity);
            };
            select.OnUnSelect = selected =>
            {
                InventoryPredSize -= selected.Data.Weight;
                InventoryIndicator.OnChangeInventory(InventorySize, InventoryPredSize, InventoryCapacity);
            };
            return select;
        }
    }
}
