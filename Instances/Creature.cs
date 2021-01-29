using Godot;
using MiniAbyss.Hud;
using MiniAbyss.StatusEffects;

namespace MiniAbyss.Instances
{
    public class Creature : Entity
    {
        [Export] public Faction Faction;
        [Export] public PackedScene DamagePopTextScene;

        public AnimationPlayer ReactionAnimationPlayer;
        public StatusManager StatusManager;
        public int MaxHealth = 5;
        public int Health = 5;
        public int Strength;
        public int Defence;

        public override void _Ready()
        {
            base._Ready();
            StatusManager = new StatusManager(this);
            ReactionAnimationPlayer = GetNode<AnimationPlayer>("ReactionAnimationPlayer");
        }

        public virtual void OnActionFinished()
        {
            AnimationPlayer.Play("Idle");
        }

        public async void Move(Vector2 dir)
        {
            AnimationPlayer.Play("Move");
            var initialVal = SpritePivot.Position - dir * SpriteDimension;
            var finalVal = SpritePivot.Position;
            Tween.InterpolateProperty(SpritePivot, "position", initialVal, finalVal,
                AnimationPlayer.CurrentAnimationLength, Tween.TransitionType.Quad);
            Tween.Start();
            Position += dir * SpriteDimension;
            SpritePivot.Position = initialVal;
            await ToSignal(AnimationPlayer, "animation_finished");
            OnActionFinished();
        }

        public async void Attack(Creature target)
        {
            AnimationPlayer.Play("Move");
            var dir = (Position - target.Position).Normalized();
            var initialVal = SpritePivot.Position;
            var intermediateVal = SpritePivot.Position - dir * SpriteDimension * 0.5f;
            var finalVal = initialVal;
            var firstDuration = AnimationPlayer.CurrentAnimationLength / 4;
            var secondDuration = AnimationPlayer.CurrentAnimationLength / 4 * 3;
            Tween.InterpolateProperty(SpritePivot, "position", initialVal, intermediateVal,
                firstDuration, Tween.TransitionType.Bounce);
            Tween.InterpolateProperty(SpritePivot, "position", intermediateVal, finalVal,
                secondDuration, Tween.TransitionType.Bounce, Tween.EaseType.InOut, firstDuration);
            Tween.Start();
            Damage(target);
            await ToSignal(AnimationPlayer, "animation_finished");
            OnActionFinished();
        }

        public virtual void Damage(Creature target)
        {
            var amount = Strength;
            target.Hit(amount, this);
        }

        public virtual void Heal(int amount)
        {
            var finalAmount = amount;
            Health = Mathf.Min(Health + finalAmount, MaxHealth);

            var popText = (DamagePopText) DamagePopTextScene.Instance();
            GetTree().CurrentScene.AddChild(popText);
            popText.Pop(finalAmount, GlobalPosition);
        }

        public virtual async void Hit(int amount, Creature dealer, bool pure = false)
        {
            ReactionAnimationPlayer.Play("Move");
            var finalDmgAmount = pure ? amount : Mathf.Max(1, amount - Defence);
            Health -= finalDmgAmount;

            var popText = (DamagePopText) DamagePopTextScene.Instance();
            GetTree().CurrentScene.AddChild(popText);
            popText.Pop(-finalDmgAmount, GlobalPosition);

            DeathCheck();
        }

        public override string MakeDescription()
        {
            return $"{base.MakeDescription()}\nATK {Strength} | DEF {Defence}";
        }

        public async void Bump()
        {
            AnimationPlayer.Play("Bump");
            await ToSignal(AnimationPlayer, "animation_finished");
            OnActionFinished();
        }

        public bool IsDead()
        {
            return Health <= 0;
        }

        public void DeathCheck()
        {
            if (IsDead()) OnDeath();
        }

        public async virtual void OnDeath()
        {
            AnimationPlayer.Play("Bump");
            var initialVal = AnimatedSprite.Modulate;
            var finalVal = Colors.Transparent;
            Tween.InterpolateProperty(AnimatedSprite, "modulate", initialVal, finalVal, AnimationPlayer.CurrentAnimationLength);
            Tween.Start();
            await ToSignal(AnimationPlayer, "animation_finished");
            BattleGrid.RemoveEntity(this);
            QueueFree();
        }
    }
}
