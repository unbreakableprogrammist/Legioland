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
    private GameModel _gameModel;
    private TcpListener _listener;
    private List<TcpClient>_clients = new List<TcpClient>();
    
    private readonly object _lock = new object(); // ala mutex 
    private Dictionary<string, Func<ClientActionDto, Player, ICommand>> _commandFactory;// nazwa akci -> funkcja 
    
    public GameServer(GameModel model)
    {
        _gameModel = model;
        
        _commandFactory = new Dictionary<string, Func<ClientActionDto, Player, ICommand>>
        {
            { "MOVE", (dto, p) => p.IsInCombatMode ? null : new MoveCommand(p, _gameModel.Dungeon, dto.Dx, dto.Dy) },
            { "PICKUP", (dto, p) => p.IsInCombatMode ? null : new PickUpCommand(p, _gameModel.Dungeon, _gameModel.SoundNetwork) },
            { "DROP", (dto, p) => p.IsInCombatMode ? null : new DropCommand(p, _gameModel.Dungeon) },
            
            { "ACTION_L", (dto, p) => p.IsInCombatMode 
                ? new AttackCommand(p, _gameModel.Dungeon, false) 
                : new EquipCommand(p, false) },
            { "ACTION_R", (dto, p) => p.IsInCombatMode 
                ? new AttackCommand(p, _gameModel.Dungeon, true) 
                : new EquipCommand(p, true) },
            
            { "INV_UP", (dto, p) => new InventoryUpCommand(p) },
            { "INV_DOWN", (dto, p) => new InventoryDownCommand(p) },
            { "GND_LEFT", (dto, p) => new GroundSelectLeftCommand(p, _gameModel.Dungeon) },
            { "GND_RIGHT", (dto, p) => new GroundSelectRightCommand(p, _gameModel.Dungeon) },
            
            { "TOGGLE_COMBAT", (dto, p) => new ToggleCombatCommand(p, _gameModel.Dungeon) },
            { "STYLE_1", (dto, p) => new ChangeStyleCommand(p, new AtakZwyklyVisitor()) },
            { "STYLE_2", (dto, p) => new ChangeStyleCommand(p, new AtakSkrytyVisitor()) },
            { "STYLE_3", (dto, p) => new ChangeStyleCommand(p, new AtakMagicznyVisitor()) }
        };
    }
    public void Start(int port)
    {
        _listener = new TcpListener(IPAddress.Any, port);
        _listener.Start();
        Console.WriteLine($"[SERWER] Uruchomiono na porcie {port}. Oczekuję na graczy...");

        Task.Run(EnemyLoop);

        int playerIdCounter = 1;

        while (true)
        {
            TcpClient client = _listener.AcceptTcpClient();
            bool isServerFull;
            lock (_lock)
            {
                isServerFull = _clients.Count >= 9;
            }

            if (isServerFull)
            {
                client.Close();
                continue;
            }
                
            int newPlayerId = playerIdCounter++;
            StreamWriter writer = new StreamWriter(client.GetStream()) { AutoFlush = true };
            writer.WriteLine(newPlayerId.ToString());

            Console.WriteLine($"[SERWER] Dołączył Gracz {newPlayerId}!");
            Gra.Logging.Logger.Instance.Log($"Gracz {newPlayerId} dołączył do gry!");

            lock (_lock)
            {
                _clients.Add(client);
                _gameModel.AddPlayer(newPlayerId, new Player(0, 0));
            }

            Task.Run(() => HandleClient(client, newPlayerId));
                
            BroadcastState(); 
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
                    if (json == null) break;

                    ClientActionDto action = JsonSerializer.Deserialize<ClientActionDto>(json);
                    
                    lock (_lock)
                    {
                        Player p = _gameModel.Players[playerId];
                        
                        if (_commandFactory.TryGetValue(action.ActionType, out var commandCreator))
                        {
                            ICommand cmd = commandCreator(action, p);
                            if (cmd != null)
                            {
                                cmd.Execute();
                            }
                        }
                    }
                    BroadcastState();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SERWER] Utracono połączenie z Graczem {playerId}.");
            }
            finally
            {
                Gra.Logging.Logger.Instance.Log($"Gracz {playerId} opuścił serwer.");

                lock (_lock)
                {
                    _gameModel.RemovePlayer(playerId);
                    _clients.Remove(client);
                }
                client.Close();
                BroadcastState();
            }
        }

        private async Task EnemyLoop()
        {
            while (true)
            {
                await Task.Delay(2000);
                lock (_lock)
                {
                    _gameModel.MoveAllEnemies();
                }
                BroadcastState();
            }
        }

        private void BroadcastState()
        {
            GameStateDto dto;
            lock (_lock)
            {
                dto = _gameModel.ToDto();
            }

            string json = JsonSerializer.Serialize(dto);

            lock (_lock)
            {
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
                    }
                }
            }
        }
}
