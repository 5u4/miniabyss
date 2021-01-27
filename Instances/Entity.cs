using Godot;
using MiniAbyss.Games;

namespace MiniAbyss.Instances
{
    public abstract class Entity : Node2D
    {
        [Export] public NodePath BattleGridPath;

        public const int SpriteDimension = 8;

        public BattleGrid BattleGrid;
        public Node2D SpritePivot;
        public Sprite Sprite;
        public AnimationPlayer AnimationPlayer;
        public Tween Tween;

        // Stats
        public int Health;
        public int Strength;
        public int Defence;

        public override void _Ready()
        {
            BattleGrid = GetNode<BattleGrid>(BattleGridPath);
            SpritePivot = GetNode<Node2D>("SpritePivot");
            Sprite = GetNode<Sprite>("SpritePivot/Sprite");
            AnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
            Tween = GetNode<Tween>("Tween");

            AnimationPlayer.Play("Idle");
        }
    }
}
