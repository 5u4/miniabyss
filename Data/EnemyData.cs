using System.Collections.Generic;
using Godot;

namespace MiniAbyss.Data
{
    public class EnemyData
    {
        public static Dictionary<EnemyKind, EnemyData> Dictionary = new Dictionary<EnemyKind, EnemyData>
        {
            {
                EnemyKind.Skull,
                new EnemyData
                {
                    Display = "Skull",
                    Health = 3,
                    Strength = 1,
                    Defence = 0,
                    SpriteFrames = GD.Load("res://SpriteFrames/SkullSpriteFrames.tres"),
                }
            },
        };

        public string Display;
        public int Health;
        public int Strength;
        public int Defence;
        public Resource SpriteFrames;
    }
}
