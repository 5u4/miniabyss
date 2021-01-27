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
        public AnimatedSprite AnimatedSprite;
        public AnimationPlayer AnimationPlayer;
        public Tween Tween;

        public override void _Ready()
        {
            BattleGrid = GetNode<BattleGrid>(BattleGridPath);
            SpritePivot = GetNode<Node2D>("SpritePivot");
            AnimatedSprite = GetNode<AnimatedSprite>("SpritePivot/Sprite");
            AnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
            Tween = GetNode<Tween>("Tween");
            AnimatedSprite.Play();

            AnimationPlayer.Play("Idle");
        }
    }
}
