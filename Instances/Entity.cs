using Godot;
using MiniAbyss.Games;
using MiniAbyss.Hud;

namespace MiniAbyss.Instances
{
    public abstract class Entity : Node2D
    {
        [Export] public NodePath BattleGridPath;
        [Export] public Resource SpriteFramesResource;
        [Export] public string Display;
        [Export] public string ShortDesc;

        public const int SpriteDimension = 8;

        public BattleGrid BattleGrid;
        public Node2D SpritePivot;
        public AnimatedSprite AnimatedSprite;
        public AnimationPlayer AnimationPlayer;
        public Tween Tween;
        public DescriptionArea DescriptionArea;

        public override void _Ready()
        {
            BattleGrid = GetNode<BattleGrid>(BattleGridPath);
            SpritePivot = GetNode<Node2D>("SpritePivot");
            AnimatedSprite = GetNode<AnimatedSprite>("SpritePivot/Sprite");
            AnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
            Tween = GetNode<Tween>("Tween");
            DescriptionArea = GetNode<DescriptionArea>("DescriptionArea");

            AnimatedSprite.Frames = (SpriteFrames) SpriteFramesResource;
            AnimatedSprite.Play();

            AnimationPlayer.Play("Idle");

            DescriptionArea.BeforeShow = () => DescriptionArea.Description = MakeDescription();
        }

        public virtual string MakeDescription()
        {
            return $"{Display}: {ShortDesc}";
        }
    }
}
