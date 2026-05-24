using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using Gra.Model;
using Gra.Movement;
using Gra.Network.DTO;

namespace Gra.Network;

public class GameServer
{
    private GameModel _gameModel; // to jest ten obiekt ktory pakuje rzeczy do GameState i wysyla do wydrukowania
    private TcpListener _listener;
    private List<TcpClient>_clients = new List<TcpClient>();
    
    private readonly object _lock = new object(); // taki ala mutex csharpowy 
    private Dictionary<string, Func<ClientActionDto, Player, ICommand>> _commandFactory;
    
    public GameServer(GameModel model)
    {
        _gameModel = model;
            
        // Rejestrujemy komendy
        _commandFactory = new Dictionary<string, Func<ClientActionDto, Player, ICommand>>
        {
            { "MOVE", (dto, p) => new MoveCommand(p, _gameModel.Dungeon, dto.Dx, dto.Dy) },
            { "PICKUP", (dto, p) => new PickUpCommand(p, _gameModel.Dungeon, _gameModel.SoundNetwork) },
            { "DROP", (dto, p) => new DropCommand(p, _gameModel.Dungeon) },
            { "EQUIP_L", (dto, p) => new EquipCommand(p, false) },
            { "EQUIP_R", (dto, p) => new EquipCommand(p, true) },
            { "INV_UP", (dto, p) => new InventoryUpCommand(p) },
            { "INV_DOWN", (dto, p) => new InventoryDownCommand(p) },
            { "GND_LEFT", (dto, p) => new GroundSelectLeftCommand(p, _gameModel.Dungeon) },
            { "GND_RIGHT", (dto, p) => new GroundSelectRightCommand(p, _gameModel.Dungeon) }
        };
    }
    public void Start(int port)
    {
        _listener = new TcpListener(IPAddress.Any, port);
        _listener.Start();
        Console.WriteLine($"[SERWER] Uruchomiono na porcie {port}. Oczekuję na graczy...");

        // Wątek, który co 2 sekundy rusza przeciwnikami i rozsyła nowy stan do graczy
        Task.Run(EnemyLoop);

        int playerIdCounter = 1;

        // Główna pętla serwera - akceptuje nowych graczy (max 9)
        while (true)
        {
            if (_clients.Count < 9)
            {
                TcpClient client = _listener.AcceptTcpClient();
                _clients.Add(client);
                    
                int newPlayerId = playerIdCounter++;
                Console.WriteLine($"[SERWER] Dołączył Gracz {newPlayerId}!");

                lock (_lock)
                {
                    // Dodajemy gracza do modelu na polu (0,0) - możesz to zmienić na inne współrzędne
                    _gameModel.AddPlayer(newPlayerId, new Player(0, 0));
                }

                // Rozpoczynamy nasłuchiwanie tego gracza w tle (osobny Task dla każdego gracza!)
                Task.Run(() => HandleClient(client, newPlayerId));
                    
                // Rozsyłamy nowy stan wszystkim, bo gracz dołączył
                BroadcastState(); 
            }
            else
            {
                // Serwer pełny, odrzucamy połączenie
                TcpClient rejected = _listener.AcceptTcpClient();
                rejected.Close();
            }
        }
    }
    private void HandleClient(TcpClient client, int playerId)
        {
            try
            {
                StreamReader reader = new StreamReader(client.GetStream());
                while (true)
                {
                    string json = reader.ReadLine();
                    if (json == null) break; // Klient się rozłączył

                    ClientActionDto action = JsonSerializer.Deserialize<ClientActionDto>(json);
                    
                    // Używamy zablokowania _stateLock, by nikt inny nie ruszał modelu w tym czasie
                    lock (_lock)
                    {
                        Player p = _gameModel.Players[playerId];
                        
                        // Zamiast IF'a, pytamy fabryki o odpowiedni obiekt ICommand
                        if (_commandFactory.TryGetValue(action.ActionType, out var commandCreator))
                        {
                            ICommand cmd = commandCreator(action, p);
                            cmd.Execute();
                        }
                    }
                    // Po wykonaniu akcji rozsyłamy stan gry
                    BroadcastState();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SERWER] Utracono połączenie z Graczem {playerId}.");
            }
            finally
            {
                lock (_lock)
                {
                    _gameModel.RemovePlayer(playerId);
                    _clients.Remove(client);
                }
                client.Close();
                BroadcastState(); // Odświeżamy mapę bez tego gracza
            }
        }

        // Metoda, która budzi przeciwników do ruchu
        private async Task EnemyLoop()
        {
            while (true)
            {
                await Task.Delay(2000); // Co 2 sekundy ruch wrogów
                lock (_lock)
                {
                    _gameModel.MoveAllEnemies();
                }
                BroadcastState();
            }
        }

        // Metoda rozsyłająca DTO wszystkim podłączonym graczom
        private void BroadcastState()
        {
            GameStateDto dto;
            lock (_lock)
            {
                dto = _gameModel.ToDto();
            }

            string json = JsonSerializer.Serialize(dto);

            // Wysłanie jsona do każdego klienta
            foreach (var client in _clients)
            {
                try
                {
                    StreamWriter writer = new StreamWriter(client.GetStream());
                    writer.WriteLine(json);
                    writer.Flush();
                }
                catch
                {
                    // Ignorujemy, rozłączenie zostanie obsłużone w HandleClient
                }
            }
        }
}