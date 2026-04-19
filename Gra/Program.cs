using System;
using Gra.Map; 

namespace Gra
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            IDungeonBuilder builder = new DungeonBuilder();
            DungeonDirector director = new DungeonDirector();
            Dungeon dungeon = director.BuildLegioland(builder, 25, 15);

            Player player = new Player(0, 0);

            GameManager engine = new GameManager(player, dungeon);
            
            engine.StartGame();
        }
    }
}