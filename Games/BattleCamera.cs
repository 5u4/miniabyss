using Godot;

namespace MiniAbyss.Games
{
    public class BattleCamera : Camera2D
    {
        [Export] public NodePath BattleGridPath;
        public BattleGrid BattleGrid;

        public override void _Ready()
        {
            BattleGrid = GetNode<BattleGrid>(BattleGridPath);

            BattleGrid.Connect(nameof(BattleGrid.OnGenerateMap), this, nameof(SetLimitsBasedOnBattleGrid));
            SetLimitsBasedOnBattleGrid();
        }

        private void SetLimitsBasedOnBattleGrid()
        {
            var gRect = BattleGrid.GetUsedRect();
            var vRect = GetViewportRect();
            var csize = BattleGrid.CellSize;
            LimitLeft = Mathf.FloorToInt(Mathf.Min(vRect.Position.x, gRect.Position.x * csize.x));
            LimitRight = Mathf.FloorToInt(Mathf.Max(vRect.End.x, gRect.End.x * csize.x));
            LimitTop = Mathf.FloorToInt(Mathf.Min(vRect.Position.y, gRect.Position.y * csize.y));
            LimitBottom = Mathf.FloorToInt(Mathf.Max(vRect.End.y, gRect.End.y * csize.y));
        }
    }
}
