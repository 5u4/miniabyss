using Godot;
using MiniAbyss.Instances;

namespace MiniAbyss.Games
{
    public class BattleGrid : TileMap
    {
        public void HandleAction(Entity e, Vector2 dir)
        {
            // TODO
            e.Move(dir);
        }
    }
}
