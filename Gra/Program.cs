using System;
using System.Text.Json;
using Gra.Config;
using Gra.Map;
using Gra.Map.Themes;
using Gra.Logging;
using Gra.Model;
using Gra.Network;
using Gra.Observer;
using Gra.View;

namespace Gra
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            
            Console.WriteLine("Wybierz tryb uruchomienia LEGIOLANDU:");
            Console.WriteLine("1. Uruchom jako SERWER (Host, tworzy mapę)");
            Console.WriteLine("2. Uruchom jako KLIENT (Gracz, łączy się do gry)");
            Console.Write("Wybór: ");
            
            string choice = Console.ReadLine();

            if (choice == "1")
            {
                RunServer();
            }
            else if (choice == "2")
            {
                RunClient();
            }
            else
            {
                Console.WriteLine("Nieznany wybór. Zamykam...");
            }
        }

        static void RunServer()
        {
            // 1. Ładowanie konfiguracji
            string jsonString = File.ReadAllText("config.json");
            var config = JsonSerializer.Deserialize<GameConfig>(jsonString);
            
            Logger.Instance.SetStrategy(new FileLoggerStrategy());
            Logger.Instance.Log($"--- START NOWEJ SESJI SERWERA ---");
            
            // 2. Setup sieci powiadomień i motywu
            ISubject<SoundPayload> globalSoundNet = new NotificationSubject<SoundPayload>();
            ISubject<DeathPayload> clubsNet = new NotificationSubject<DeathPayload>();
            ISubject<DeathPayload> officialsNet = new NotificationSubject<DeathPayload>();
            
            IThemeFactory factory = config.Theme switch
            {
                "Puchary" => new EuropeanCupFactory(),
                "Mistrzostwo" => new MasterThemeFactory(),
                "Utrzymanie" => new RelegationThemeFactory(),
                _ => new MasterThemeFactory() 
            };

            // 3. Budowanie mapy (Tylko na serwerze!)
            IDungeonBuilder builder = new DungeonBuilder(factory, globalSoundNet, clubsNet, officialsNet); 
            DungeonDirector director = new DungeonDirector();
            Dungeon dungeon = director.BuildLegioland(builder, 25, 15);

            // 4. Inicjalizacja Modelu i Serwera
            GameModel model = new GameModel(dungeon, globalSoundNet);
            GameServer server = new GameServer(model);
            
            // 5. Uruchomienie Serwera na porcie 5555
            server.Start(5555);
        }

        static void RunClient()
        {
            // 1. Setup interfejsu klienta
            IView view = new ConsoleView();
            GameClient client = new GameClient(view);
            
            // 2. Pytanie o adres IP
            Console.WriteLine("Wpisz IP serwera (lub wciśnij ENTER, aby połączyć się z samym sobą - 127.0.0.1):");
            string ip = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(ip)) ip = "127.0.0.1";
            
            // 3. Łączenie z serwerem
            client.Connect(ip, 5555);
        }
    }
}
