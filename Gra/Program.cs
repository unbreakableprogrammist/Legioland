using System;
using System.Text.Json;
using Gra.Config;
using Gra.Map;
using Gra.Map.Themes;
using Gra.Logging;

namespace Gra
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            string jsonString = File.ReadAllText("config.json");
            var config = JsonSerializer.Deserialize<GameConfig>(jsonString);
            Logger.Instance.SetStrategy(new FileLoggerStrategy());
            Logger.Instance.Log($"--- START NOWEJ SESJI: {config.PlayerName} ---");
            IThemeFactory factory = config.Theme switch
            {
                "Puchary" => new EuropeanCupFactory(),
                "Mistrzostwo" => new MasterThemeFactory(),
                "Utrzymanie" => new RelegationThemeFactory(),
                _ => new MasterThemeFactory() 
            };

            IDungeonBuilder builder = new DungeonBuilder(factory);
            DungeonDirector director = new DungeonDirector();
            Dungeon dungeon = director.BuildLegioland(builder, 25, 15);

            Player player = new Player(0, 0);
            GameManager engine = new GameManager(player, dungeon, factory, config);
    
            engine.StartGame();
        }
    }
}
