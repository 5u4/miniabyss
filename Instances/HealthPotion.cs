namespace MiniAbyss.Instances
{
    public class HealthPotion : Consumable
    {
        public int Amount = 5;

        public override void Consume(Player player)
        {
            player.Heal(Amount);
            base.Consume(player);
        }
    }
}
