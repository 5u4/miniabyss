using System.Collections.Generic;
using Godot;
using MiniAbyss.Instances;

namespace MiniAbyss.Data
{
    public class EnemyData
    {
        public delegate void AfterDamageHook(Creature self, Creature target);

        public static Dictionary<EnemyKind, EnemyData> Dictionary = new Dictionary<EnemyKind, EnemyData>
        {
            {
                EnemyKind.Skull,
                new EnemyData
                {
                    Display = "Skull",
                    Description = "A skull without body.",
                    Health = 3,
                    Strength = 1,
                    Defence = 0,
                    SightRadius = 5,
                    SpriteFrames = GD.Load("res://SpriteFrames/SkullSpriteFrames.tres"),
                }
            },
            {
                EnemyKind.Skeleton,
                new EnemyData
                {
                    Display = "Skeleton",
                    Description = "A skull with body.",
                    Health = 5,
                    Strength = 3,
                    Defence = 1,
                    SightRadius = 5,
                    SpriteFrames = GD.Load("res://SpriteFrames/SkeletonSpriteFrames.tres"),
                }
            },
            {
                EnemyKind.Bat,
                new EnemyData
                {
                    Display = "Bat",
                    Description = "Heal 1 after attacking.",
                    Health = 4,
                    Strength = 1,
                    Defence = 0,
                    SightRadius = 6,
                    SpriteFrames = GD.Load("res://SpriteFrames/BatSpriteFrames.tres"),
                    AfterDamage = (bat, _) => bat.Heal(1),
                }
            },
            {
                EnemyKind.Ghost,
                new EnemyData
                {
                    Display = "Ghost",
                    Description = "Reduce target 1 defence after attacking.",
                    Health = 10,
                    Strength = 4,
                    Defence = 0,
                    SightRadius = 10,
                    SpriteFrames = GD.Load("res://SpriteFrames/GhostSpriteFrames.tres"),
                    AfterDamage = (_, target) => target.Defence -= 1,
                }
            }
        };

        public string Display;
        public string Description;
        public int Health;
        public int Strength;
        public int Defence;
        public Resource SpriteFrames;
        public int SightRadius;
        public AfterDamageHook AfterDamage;
    }
}
