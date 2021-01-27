using Godot;
using MiniAbyss.Games;

namespace MiniAbyss.Instances
{
    public abstract class Entity : Node2D
    {
        [Export] public NodePath BattleGridPath;

        public BattleGrid BattleGrid;
        public Node2D SpritePivot;
        public Sprite Sprite;
        public AnimationPlayer AnimationPlayer;
        public Tween Tween;

        // Stats
        public int Health;
        public int Attack;
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

        public async void Move(Vector2 dir)
        {
            SetProcess(false);
            AnimationPlayer.Play("Move");
            var initialVal = SpritePivot.Position - dir * 8;
            var finalVal = SpritePivot.Position;
            Tween.InterpolateProperty(SpritePivot, "position", initialVal, finalVal,
                AnimationPlayer.CurrentAnimationLength);
            Tween.Start();
            Position += dir * 8;
            SpritePivot.Position = initialVal;
            await ToSignal(AnimationPlayer, "animation_finished");
            SetProcess(true);
            AnimationPlayer.Play("Idle");
        }
    }
}
