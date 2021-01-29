using MiniAbyss.Hud;

namespace MiniAbyss.StatusEffects
{
    public class BleedIcon : Icon
    {
        public BleedStatus Handler;

        public override void Rerender()
        {
            Visible = !Handler.CanRemove();
            Count.Text = Handler.TotalDamage.ToString();
        }
    }
}
