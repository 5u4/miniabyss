using MiniAbyss.Hud;

namespace MiniAbyss.StatusEffects
{
    public class BurnIcon : Icon
    {
        public BurnStatus Handler;

        public override void Rerender()
        {
            Visible = !Handler.CanRemove();
            Count.Text = Handler.Turn.ToString();
        }
    }
}
