using Godot;

namespace MiniAbyss.Games
{
    public class BattleCamera : Camera2D
    {
        [Export] public NodePath FollowingPath;

        public Node2D Following;

        public override void _Ready()
        {
            Following = GetNode<Node2D>(FollowingPath);
        }

        public override void _PhysicsProcess(float delta)
        {
            if (Following != null) GlobalPosition = Following.GlobalPosition;
        }
    }
}
