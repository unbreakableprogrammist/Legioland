using System;
using System.Text.Json;
using Gra.Config;
using Gra.Map;
using Gra.Map.Themes;

namespace Gra
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            string jsonString = File.ReadAllText("config.json");
            var config = JsonSerializer.Deserialize<GameConfig>(jsonString);
            IThemeFactory factory = config.Theme switch
            {
                "Puchary" => new EuropeanCupFactory(),
                "Mistrzostwo" => new MasterThemeFactory(),
                "Utrzymanie" => new RelegationThemeFactory()
            };
            IDungeonBuilder builder = new DungeonBuilder(factory);
            DungeonDirector director = new DungeonDirector();
            Dungeon dungeon = director.BuildLegioland(builder, 25, 15);

            Player player = new Player(0, 0);

            GameManager engine = new GameManager(player, dungeon,factory);
            
            engine.StartGame();
        }
    }
}
