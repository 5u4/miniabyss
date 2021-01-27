using Godot;
using MiniAbyss.Hud;

namespace MiniAbyss.Instances
{
    public class Creature : Entity
    {
        [Export] public Faction Faction;
        [Export] public PackedScene DamagePopTextScene;

        public int Health = 5;
        public int Strength;
        public int Defence;

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

        public void Damage(Creature target)
        {
            var amount = Strength;
            target.Hit(amount, this);
        }

        public async void Hit(int amount, Creature dealer)
        {
            AnimationPlayer.Play("Move");
            var finalDmgAmount = Mathf.Max(1, amount - Defence);
            Health -= finalDmgAmount;

            var popText = (DamagePopText) DamagePopTextScene.Instance();
            GetTree().CurrentScene.AddChild(popText);
            popText.Pop(-finalDmgAmount, GlobalPosition);

            await ToSignal(AnimationPlayer, "animation_finished");
            AnimationPlayer.Play("Idle");
            DeathCheck();
        }

        public async void Bump()
        {
            AnimationPlayer.Play("Bump");
            await ToSignal(AnimationPlayer, "animation_finished");
            OnActionFinished();
        }

        public void DeathCheck()
        {
            if (Health <= 0) OnDeath();
        }

        public void OnDeath()
        {
            // TODO
            GD.Print("Death");
        }
    }
}
