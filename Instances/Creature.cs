using Godot;

namespace MiniAbyss.Instances
{
    public class Creature : Entity
    {
        [Export] public Faction Faction;

        public async void Move(Vector2 dir)
        {
            SetProcess(false);
            AnimationPlayer.Play("Move");
            var initialVal = SpritePivot.Position - dir * SpriteDimension;
            var finalVal = SpritePivot.Position;
            Tween.InterpolateProperty(SpritePivot, "position", initialVal, finalVal,
                AnimationPlayer.CurrentAnimationLength, Tween.TransitionType.Quad);
            Tween.Start();
            Position += dir * SpriteDimension;
            SpritePivot.Position = initialVal;
            await ToSignal(AnimationPlayer, "animation_finished");
            SetProcess(true);
            AnimationPlayer.Play("Idle");
        }

        public async void Attack(Creature target)
        {
            SetProcess(false);
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
            await ToSignal(Tween, "tween_all_completed");
            SetProcess(true);
            AnimationPlayer.Play("Idle");
        }

        public void Damage(Creature target)
        {
            var amount = Strength;
            target.Hit(amount, this);
        }

        private async void Hit(int amount, Creature dealer)
        {
            AnimationPlayer.Play("Move");
            Health -= Mathf.Max(1, amount - Defence);
            await ToSignal(AnimationPlayer, "animation_finished");
            AnimationPlayer.Play("Idle");
            DeathCheck();
        }

        public async void Bump()
        {
            SetProcess(false);
            AnimationPlayer.Play("Bump");
            await ToSignal(AnimationPlayer, "animation_finished");
            SetProcess(true);
            AnimationPlayer.Play("Idle");
        }

        private void DeathCheck()
        {
            if (Health <= 0) OnDeath();
        }

        private void OnDeath()
        {
            // TODO
            GD.Print("Death");
        }
    }
}
