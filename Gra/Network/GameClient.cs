using System.Net.Sockets;
using System.Text.Json;
using Gra.Network.DTO;
using Gra.View;

namespace Gra.Network;

public class GameClient
{
    private TcpClient _client;
    private IView _view;
    private StreamWriter _writer;
    private StreamReader _reader;

    private int _myPlayerId = -1;
    private bool _showLogs = false;

    public GameClient(IView view)
    {
        _view = view;
    }

    public void Connect(string ip, int port)
    {
        try
        {
            _client = new TcpClient(ip, port);
            var stream = _client.GetStream();
            _writer = new StreamWriter(stream) { AutoFlush = true };
            _reader = new StreamReader(stream);
            _myPlayerId = int.Parse(_reader.ReadLine());

            Console.WriteLine("Połączono z serwerem! Oczekiwanie na stan gry...");
            Console.Clear();
            _view.ShowIntro("Witaj w Legiolandzie! Czas rozpocząć mecz...");
            Console.CursorVisible = false;

            Task.Run(ListenToServer);

            InputLoop();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Nie udało się połączyć: {ex.Message}");
        }
    }

    private void InputLoop()
    {
        while (true)
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);

            if (keyInfo.Key == ConsoleKey.J)
            {
                _showLogs = !_showLogs;
                Console.Clear();
                continue;
            }

            if (keyInfo.Key == ConsoleKey.Q)
            {
                Console.CursorVisible = true; 
                Environment.Exit(0);
            }

            ClientActionDto action = null;

            switch (keyInfo.Key)
            {
                case ConsoleKey.W: action = new ClientActionDto { ActionType = "MOVE", Dx = 0, Dy = -1 }; break;
                case ConsoleKey.S: action = new ClientActionDto { ActionType = "MOVE", Dx = 0, Dy = 1 }; break;
                case ConsoleKey.A: action = new ClientActionDto { ActionType = "MOVE", Dx = -1, Dy = 0 }; break;
                case ConsoleKey.D: action = new ClientActionDto { ActionType = "MOVE", Dx = 1, Dy = 0 }; break;
                case ConsoleKey.E: action = new ClientActionDto { ActionType = "PICKUP" }; break;
                case ConsoleKey.F: action = new ClientActionDto { ActionType = "DROP" }; break;
                
                case ConsoleKey.L: action = new ClientActionDto { ActionType = "ACTION_L" }; break;
                case ConsoleKey.R: action = new ClientActionDto { ActionType = "ACTION_R" }; break;
                
                case ConsoleKey.UpArrow: action = new ClientActionDto { ActionType = "INV_UP" }; break;
                case ConsoleKey.DownArrow: action = new ClientActionDto { ActionType = "INV_DOWN" }; break;
                case ConsoleKey.LeftArrow: action = new ClientActionDto { ActionType = "GND_LEFT" }; break;
                case ConsoleKey.RightArrow: action = new ClientActionDto { ActionType = "GND_RIGHT" }; break;
                
                case ConsoleKey.X: action = new ClientActionDto { ActionType = "TOGGLE_COMBAT" }; break;
                case ConsoleKey.D1: action = new ClientActionDto { ActionType = "STYLE_1" }; break;
                case ConsoleKey.D2: action = new ClientActionDto { ActionType = "STYLE_2" }; break;
                case ConsoleKey.D3: action = new ClientActionDto { ActionType = "STYLE_3" }; break;
                case ConsoleKey.Y: action = new ClientActionDto { ActionType = "SLOT_L" }; break;
                case ConsoleKey.U: action = new ClientActionDto { ActionType = "SLOT_R" }; break;
            }

            if (action != null && _myPlayerId != -1)
            {
                action.PlayerId = _myPlayerId;
                string json = JsonSerializer.Serialize(action);
                _writer.WriteLine(json);
            }
        }
    }

    private async Task ListenToServer()
    {
        try
        {
            while (true)
            {
                string json = await _reader.ReadLineAsync();
                if (json == null) break;
 
                GameStateDto state = JsonSerializer.Deserialize<GameStateDto>(json);

                PlayerDto myPlayer = state.Players.FirstOrDefault(p => p.Id == _myPlayerId);
                if (myPlayer != null && myPlayer.Health <= 0)
                {
                    System.IO.File.WriteAllLines("logs.txt", state.Logs);

                    _view.ShowGameOver($"Gracz {_myPlayerId}", myPlayer.Points, myPlayer.Goals, "logs.txt", true);
                    Console.CursorVisible = true;
                    Environment.Exit(0);
                }

                string currentStatus = myPlayer?.StatusMessage ?? "";

                _view.Render(state, _myPlayerId, currentStatus, _showLogs);
            }
        }
        catch (Exception)
        {
            Console.Clear();
            Console.WriteLine("Rozłączono z serwerem.");
        }
    }
}
