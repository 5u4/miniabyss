using System.Collections.Generic;
using Godot;
using MiniAbyss.Instances;

namespace MiniAbyss.Data
{
    public class EnemyData
    {
        public delegate void AfterDamageHook(Creature target);

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
                    SightRadius = 5,
                    SpriteFrames = GD.Load("res://SpriteFrames/SkullSpriteFrames.tres"),
                }
            },
        };

        public string Display;
        public int Health;
        public int Strength;
        public int Defence;
        public Resource SpriteFrames;
        public int SightRadius;
        public AfterDamageHook AfterDamage;
    }
}
