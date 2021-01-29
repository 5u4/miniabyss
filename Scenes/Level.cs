using Godot;
using MiniAbyss.Data;
using MiniAbyss.Games;
using MiniAbyss.Hud;
using MiniAbyss.Instances;
using MiniAbyss.Items;

namespace MiniAbyss.Scenes
{
    public class Level : Node2D
    {
        [Export] public PackedScene HudScene;

        public static int LevelProgress = 0;

        public BattleGrid BattleGrid;
        public Player Player;

        public override void _Ready()
        {
            BattleGrid = GetNode<BattleGrid>("BattleGrid");
            Player = GetNode<Player>("BattleGrid/Player");

            if (PlayerData.Instance.HasHudItem)
            {
                var hud = HudScene.Instance();
                AddChild(hud);
                BattleGrid.StatusContainer = hud.GetNode<StatusContainer>("TopBar/HealthMoney/StatusContainer");
                BattleGrid.StatusContainer.Creature = Player;
            }
            MakeItems();
            InitializeLevel();
        }

        private void InitializeLevel()
        {
            BattleGrid.Generate(5, 1, 0.5f);
        }

        private void MakeItems()
        {
            var parent = GetNode(PlayerData.Instance.HasHudItem ? "Hud/TopBar/ItemContainer" : "ItemHolder");
            PlayerData.Instance.Inventory.ForEach(data =>
            {
                var item = Item.MakeFromData(data);
                parent.AddChild(item);
                item.Data.Apply(Player);
            });
        }
    }
}
