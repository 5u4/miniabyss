using Godot;
using MiniAbyss.Games;
using MiniAbyss.Instances;

namespace MiniAbyss.Scenes
{
    public class Level : Node2D
    {
        public static int LevelProgress = 0;

        public BattleGrid BattleGrid;
        public Player Player;

        public override void _Ready()
        {
            BattleGrid = GetNode<BattleGrid>("BattleGrid");
            Player = GetNode<Player>("BattleGrid/Player");

            InitializeLevel();
        }

        private void InitializeLevel()
        {
            BattleGrid.Generate(5, 1, 0.5f);
        }
    }
}
