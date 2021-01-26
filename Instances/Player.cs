using Godot;
using MiniAbyss.Games;

namespace MiniAbyss.Instances
{
    public class Player : Entity
    {
        [Export] public NodePath BattleGridPath;
        public BattleGrid BattleGrid;
        public Node2D SpritePivot;
        public Sprite Sprite;
        public AnimationPlayer AnimationPlayer;
        public Tween Tween;

        public override void _Ready()
        {
            BattleGrid = GetNode<BattleGrid>(BattleGridPath);
            SpritePivot = GetNode<Node2D>("SpritePivot");
            Sprite = GetNode<Sprite>("SpritePivot/Sprite");
            AnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
            Tween = GetNode<Tween>("Tween");

            AnimationPlayer.Play("Idle");
        }

        public override void _Process(float delta)
        {
            CaptureInputs();
        }

        public override async void Move(Vector2 dir)
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

        private void CaptureInputs()
        {
            if (Input.IsActionPressed("move_left")) HandleMovement(Vector2.Left);
            else if (Input.IsActionPressed("move_right")) HandleMovement(Vector2.Right);
            else if (Input.IsActionPressed("move_up")) HandleMovement(Vector2.Up);
            else if (Input.IsActionPressed("move_down")) HandleMovement(Vector2.Down);
        }

        private void HandleMovement(Vector2 dir)
        {
            BattleGrid.HandleAction(this, dir);
        }
    }
}
