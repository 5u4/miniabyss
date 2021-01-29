using MiniAbyss.Hud;

namespace MiniAbyss.StatusEffects
{
    public class PoisonIcon : Icon
    {
        public PoisonStatus Handler;

        public override void Rerender()
        {
            Visible = !Handler.CanRemove();
            Count.Text = Handler.Turn.ToString();
        }
    }
}
