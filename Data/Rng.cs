using Godot;

namespace MiniAbyss.Data
{
    public class Rng
    {
        /**
         * The game determine randomizer
         */
        public RandomNumberGenerator G;

        /**
         * The useless randomizer
         */
        public RandomNumberGenerator R;

        private Rng()
        {
            G = new RandomNumberGenerator();
            R = new RandomNumberGenerator();
        }

        public static Rng Instance { get; } = new Rng();
    }
}
