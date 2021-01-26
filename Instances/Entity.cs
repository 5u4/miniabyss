using Godot;

namespace MiniAbyss.Instances
{
    public abstract class Entity : Node2D
    {
        public abstract void Move(Vector2 dir);
    }
}
