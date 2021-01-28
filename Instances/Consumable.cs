namespace MiniAbyss.Instances
{
    public class Consumable : Entity
    {
        public override void _Ready()
        {
            base._Ready();
            AnimationPlayer.Play();
        }

        public virtual void Consume(Player player)
        {
            QueueFree();
        }
    }
}
