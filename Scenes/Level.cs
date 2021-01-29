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
        public const int BaseDim = 5;
        public const float DepthDimRatio = 0.4f;

        [Export] public PackedScene HudScene;

        public BattleGrid BattleGrid;
        public Player Player;

        public override void _Ready()
        {
            BattleGrid = GetNode<BattleGrid>("BattleGrid");
            Player = GetNode<Player>("BattleGrid/Player");

            PlayerData.Instance.Kills = 0;

            if (PlayerData.Instance.HasHudItem)
            {
                var hud = HudScene.Instance();
                AddChild(hud);
                MoveChild(hud, 0);
                BattleGrid.StatusContainer = hud.GetNode<StatusContainer>("VBox/TopBar/HealthMoney/StatusContainer");
                BattleGrid.StatusContainer.Creature = Player;
            }
            MakeItems();
            InitializeLevel();
        }

        private void InitializeLevel()
        {
            var dim = Mathf.RoundToInt(BaseDim + DepthDimRatio * PlayerData.Instance.Depth);
            BattleGrid.Generate(dim, 1, 0.5f);
        }

        private void MakeItems()
        {
            var parent = GetNode(PlayerData.Instance.HasHudItem ? "Hud/VBox/TopBar/ItemContainer" : "ItemHolder");
            PlayerData.Instance.Inventory.ForEach(data =>
            {
                var item = Item.MakeFromData(data);
                parent.AddChild(item);
                item.Data.Apply(Player);
            });
        }
    }
}
