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

        public Control LootPickerContainer;
        public Transition Transition;
        public Button NextButton;

        public List<MakeItemData> ItemDataMakers;

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

            StartTransition();

            AddHud();

            NextButton.Connect("pressed", this, nameof(OnNextButtonPressed));
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
            // TODO Next scene
        }

        private void AddHud()
        {
            var loots = GenerateLoots();
            loots.ForEach(l => LootPickerContainer.AddChild(l));
        }

        private List<ItemSelect> GenerateLoots()
        {
            var loots = new List<ItemSelect>();
            // TODO Generate by level
            for (var i = 0; i < 40; i++)
            {
                var loot = MakeOneLoot(3);
                loots.Add(loot);
            }
            return loots;
        }

        private ItemSelect MakeOneLoot(float quality)
        {
            var i = Mathf.FloorToInt(Rng.Instance.G.Randf() * Mathf.Min(ItemDataMakers.Count, quality));
            var itemData = ItemDataMakers[i].Invoke();
            var item = Item.MakeFromData(itemData);
            var select = (ItemSelect) ItemSelectScene.Instance();
            select.Item = item;
            select.AddChild(item);
            return select;
        }
    }
}
