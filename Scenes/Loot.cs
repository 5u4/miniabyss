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
        public Button NextButton;
        public List<ItemSelect> Loots;

        public List<MakeItemData> ItemDataMakers;

        public int InventorySize;
        public int InventoryCapacity;

        public override void _Ready()
        {
            InventorySize = PlayerData.Instance.GetInventorySize();
            InventoryCapacity = PlayerData.Instance.GetInventoryCapacity();

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
            select.CanSelect = selected => selected.Data.Weight + InventorySize <= InventoryCapacity;
            select.OnSelect = selected => InventorySize += selected.Data.Weight;
            select.OnUnSelect = selected => InventorySize -= selected.Data.Weight;
            return select;
        }
    }
}
