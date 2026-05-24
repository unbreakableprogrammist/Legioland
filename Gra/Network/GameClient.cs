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

    // Zmienne lokalne do rysowania UI (to też robił stary kontroler)
    private int _myPlayerId = -1; // Na razie nie znamy swojego ID, serwer nam je nada
    private bool _showLogs = false;
    private string _statusMessage = "";

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

            Console.WriteLine("Połączono z serwerem! Oczekiwanie na stan gry...");

            // 1. Wątek nasłuchujący: Odbiera mapę od Serwera i każe Widokowi ją narysować
            Task.Run(ListenToServer);

            // 2. Główny wątek: "Lekki Kontroler", który czyta klawisze i wysyła je do Serwera
            InputLoop();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Nie udało się połączyć: {ex.Message}");
        }
    }

    // --- TO JEST TWOJA DAWNA PĘTLA Z KONTROLERA (Tylko teraz wysyła w kosmos) ---
    private void InputLoop()
    {
        while (true)
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);

            // Klawisze lokalne (tylko UI), nie wysyłamy ich na serwer
            if (keyInfo.Key == ConsoleKey.J)
            {
                _showLogs = !_showLogs;
                Console.Clear();
                continue;
            }

            if (keyInfo.Key == ConsoleKey.Q)
            {
                Environment.Exit(0);
            } // Wyjście

            // Klawisze, które wymagają wysłania na Serwer
            ClientActionDto action = null;

            switch (keyInfo.Key)
            {
                case ConsoleKey.W: action = new ClientActionDto { ActionType = "MOVE", Dx = 0, Dy = -1 }; break;
                case ConsoleKey.S: action = new ClientActionDto { ActionType = "MOVE", Dx = 0, Dy = 1 }; break;
                case ConsoleKey.A: action = new ClientActionDto { ActionType = "MOVE", Dx = -1, Dy = 0 }; break;
                case ConsoleKey.D: action = new ClientActionDto { ActionType = "MOVE", Dx = 1, Dy = 0 }; break;
                case ConsoleKey.E: action = new ClientActionDto { ActionType = "PICKUP" }; break;
                case ConsoleKey.F: action = new ClientActionDto { ActionType = "DROP" }; break;
                case ConsoleKey.L: action = new ClientActionDto { ActionType = "EQUIP_L" }; break;
                case ConsoleKey.R: action = new ClientActionDto { ActionType = "EQUIP_R" }; break;
                case ConsoleKey.UpArrow: action = new ClientActionDto { ActionType = "INV_UP" }; break;
                case ConsoleKey.DownArrow: action = new ClientActionDto { ActionType = "INV_DOWN" }; break;
                case ConsoleKey.LeftArrow: action = new ClientActionDto { ActionType = "GND_LEFT" }; break;
                case ConsoleKey.RightArrow: action = new ClientActionDto { ActionType = "GND_RIGHT" }; break;
            }

            if (action != null && _myPlayerId != -1)
            {
                action.PlayerId = _myPlayerId; // Podpisujemy się, żeby serwer wiedział kto klika
                string json = JsonSerializer.Serialize(action);
                _writer.WriteLine(json); // WYSYŁAMY DO SERWERA
            }
        }
    }

    // --- WĄTEK NASŁUCHUJĄCY (Odbiera DTO z sieci i rysuje) ---
    private async Task ListenToServer()
    {
        try
        {
            while (true)
            {
                string json = await _reader.ReadLineAsync();
                if (json == null) break;

                GameStateDto state = JsonSerializer.Deserialize<GameStateDto>(json);

                // Mały trik na początku: Jeśli serwer wysłał nam graczy, a my jeszcze nie 
                // przypisaliśmy sobie ID, bierzemy najwyższe dostępne (serwer je nadaje po kolei).
                if (_myPlayerId == -1 && state.Players.Count > 0)
                {
                    _myPlayerId = state.Players[state.Players.Count - 1].Id;
                }

                // Każe ConsoleView narysować nowy stan!
                _view.Render(state, _myPlayerId, _statusMessage, _showLogs);
            }
        }
        catch (Exception)
        {
            Console.Clear();
            Console.WriteLine("Rozłączono z serwerem.");
        }
    }
}