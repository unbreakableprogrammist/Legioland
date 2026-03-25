using System;
using Gra.Map; // Tu siedzi Dungeon, Builder i Director

namespace Gra
{
    class Program
    {
        static void Main(string[] args)
        {
            IDungeonBuilder builder = new DungeonBuilder();
            DungeonDirector director = new DungeonDirector();
            Dungeon dungeon = director.BuildLegioland(builder, 25, 15);

            Player player = new Player(0, 0);

            GameManager engine = new GameManager(player, dungeon);
            
            engine.StartGame();
        }
    }
}