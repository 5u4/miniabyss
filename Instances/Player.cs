using Godot;
using MiniAbyss.Games;

namespace MiniAbyss.Instances
{
    public class Player : Entity
    {
        public override void _Process(float delta)
        {
            HandleInputs();
        }

        private void HandleInputs()
        {
            if (Input.IsActionPressed("move_left")) BattleGrid.HandleAction(this, Vector2.Left);
            else if (Input.IsActionPressed("move_right")) BattleGrid.HandleAction(this, Vector2.Right);
            else if (Input.IsActionPressed("move_up")) BattleGrid.HandleAction(this, Vector2.Up);
            else if (Input.IsActionPressed("move_down")) BattleGrid.HandleAction(this, Vector2.Down);
        }
    }
}
