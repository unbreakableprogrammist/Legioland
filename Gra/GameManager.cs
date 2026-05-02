using Gra.Map;
using Gra.Map.Themes;
using Gra.Movement;
using Gra.Logging;
using System.IO;         
using System.Threading;
using Gra.Config;

namespace Gra;


public class GameManager
{
    private bool _showLogs = false; 
    private Dictionary<ConsoleKey, ICommand> _commands;
    private Player _player;
    private Dungeon _dungeon;
    private string _statusMessage = ""; 
    IThemeFactory _factory;
    private GameConfig _config;

    public GameManager(Player player, Dungeon dungeon,IThemeFactory themeFactory,GameConfig config)
    {
        _player = player;
        _dungeon = dungeon;
        _commands = new Dictionary<ConsoleKey, ICommand>(); 
        _factory = themeFactory;
        _config = config;
        SetCommands();
    }
    
    private void SetCommands()
    {
        
        _commands[ConsoleKey.W] = new MoveCommand(_player, _dungeon, 0, -1);
        _commands[ConsoleKey.S] = new MoveCommand(_player, _dungeon, 0, 1);
        _commands[ConsoleKey.A] = new MoveCommand(_player, _dungeon, -1, 0);
        _commands[ConsoleKey.D] = new MoveCommand(_player, _dungeon, 1, 0);
        
        _commands[ConsoleKey.UpArrow] = new InventoryUpCommand(_player);
        _commands[ConsoleKey.DownArrow] = new InventoryDownCommand(_player);
        _commands[ConsoleKey.LeftArrow] = new GroundSelectLeftCommand(_player, _dungeon);
        _commands[ConsoleKey.RightArrow] = new GroundSelectRightCommand(_player, _dungeon);
        
        _commands[ConsoleKey.E] = new PickUpCommand(_player, _dungeon);
        _commands[ConsoleKey.F] = new DropCommand(_player, _dungeon);

        
        _commands[ConsoleKey.R] = new EquipCommand(_player, true);
        _commands[ConsoleKey.L] = new EquipCommand(_player, false);
    }

    
    private void ToggleCombatMode()
    {
        if (!_player.IsInCombatMode)
        {
            
            Enemy enemyOnTile = _dungeon.GetEnemyAt(_player.X, _player.Y);
            if (enemyOnTile != null)
            {
                _player.IsInCombatMode = true;
                _statusMessage = $"!!! TRYB WALKI: {enemyOnTile.Name.ToUpper()} !!!";
                _commands[ConsoleKey.R] = new AttackCommand(_player, _dungeon, true, (msg) => _statusMessage = msg);
                _commands[ConsoleKey.L] = new AttackCommand(_player, _dungeon, false, (msg) => _statusMessage = msg);
            }
            else
            {
                _statusMessage = "Tu nie ma z kim walczyć!";
            }
        }
        else
        {
            
            _player.IsInCombatMode = false;
            _statusMessage = "Tryb eksploracji.";

            
            _commands[ConsoleKey.R] = new EquipCommand(_player, true);
            _commands[ConsoleKey.L] = new EquipCommand(_player, false);
        }
    }

    public void StartGame()
    {
        ShowIntro(); 

        Console.Clear();
        Console.CursorVisible = false;

        while (true)
        {
            if (_player.Health <= 0)
            {
                ShowGameOver();
                break; 
            }
            Draw(); 
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            if (keyInfo.Key == ConsoleKey.Q)
            {
                ShowGameOver();
                break;
            }
            
            
            if (keyInfo.Key == ConsoleKey.D1) { _player.CurrentAttack = new AtakZwyklyVisitor(); _statusMessage = "Styl walki: ZWYKŁY"; continue; }
            if (keyInfo.Key == ConsoleKey.D2) { _player.CurrentAttack = new AtakSkrytyVisitor(); _statusMessage = "Styl walki: SKRYTY"; continue; }
            if (keyInfo.Key == ConsoleKey.D3) { _player.CurrentAttack = new AtakMagicznyVisitor(); _statusMessage = "Styl walki: MAGICZNY"; continue; }
            if (keyInfo.Key == ConsoleKey.X)
            {
                ToggleCombatMode();
                continue;
            }
            if (keyInfo.Key == ConsoleKey.J)
            {
                _showLogs = !_showLogs; 
                Console.Clear(); 
                continue;
            }

            if (_commands.ContainsKey(keyInfo.Key))
            {
                
                if (_player.IsInCombatMode && (keyInfo.Key == ConsoleKey.W || keyInfo.Key == ConsoleKey.S || keyInfo.Key == ConsoleKey.A || keyInfo.Key == ConsoleKey.D))
                {
                    _statusMessage = "Nie możesz się ruszyć w trakcie walki! Kliknij X, by uciec.";
                }
                else
                {
                    _commands[keyInfo.Key].Execute();
                    
                    if (!_player.IsInCombatMode) _statusMessage = ""; 
                }
            }
            else
            {
                _statusMessage = $"!!! Klawisz {keyInfo.Key} nieaktywny !!!";
            }
        }
    }
    
    private void Draw()
    {
        if (_showLogs)
        {
            DrawLogWindow();
        }
        else
        {
            DrawNormalGame(); // To jest Twoja obecna metoda Draw (cała mapa i UI)
        }
    }
    private void DrawLogWindow()
    {
        Console.SetCursorPosition(0, 0);
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("=== DZIENNIK ZDARZEŃ LEGIOLANDU (Wciśnij J, aby wrócić) ===");
        Console.ResetColor();
        Console.WriteLine("------------------------------------------------------------");

        var logs = Logger.Instance.GetLogs();
    
        // Wyświetlamy np. ostatnich 20 wpisów, żeby nie wyszło poza ekran
        int start = Math.Max(0, logs.Count - 20);
        for (int i = start; i < logs.Count; i++)
        {
            Console.WriteLine(logs[i]);
        }
    }

    private void DrawNormalGame()
    {
        Console.SetCursorPosition(0, 0);
        _dungeon.Draw(_player);

        int uiColumn = _dungeon.Width  + 20; 
        int clearWidth = 60; 

        Console.SetCursorPosition(uiColumn, 0);
        
        if (_player.IsInCombatMode)
        {
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("=== T R Y B   W A L K I ===".PadRight(clearWidth));
            Console.ResetColor();
        }
        else
        {
            Console.Write("=== LEGIOLAND (MAPA) ===".PadRight(clearWidth));
        }
        
        var itemsOnGround = _dungeon.Grid[_player.X, _player.Y].GetItemNames();
        _player.ClampGroundSelection(itemsOnGround.Count);
        
        for (int i = 0; i < Math.Max(5, itemsOnGround.Count); i++)
        {
            Console.SetCursorPosition(uiColumn, i + 1);
            if (i < itemsOnGround.Count)
            {
                string prefix = (i == _player.SelectedGroundSlot) ? "-> " : "   ";
                Console.Write($"{prefix}{itemsOnGround[i]} [Press E]".PadRight(clearWidth));
            }
            else Console.Write("".PadRight(clearWidth)); 
        }

        Console.SetCursorPosition(uiColumn, 7);
        Console.Write($"PUNKTY: {_player.Points}  GOLE: {_player.Goals}".PadRight(clearWidth));
        
        Console.SetCursorPosition(uiColumn, 8);
        Console.Write($"HP: {_player.Health}   STRENGHT : {_player.Strength}".PadRight(clearWidth));
        
        int currentLuck = _player.Luck;
        if (_player.LeftHand != null) currentLuck += _player.LeftHand.LuckModifier;
        if (_player.RightHand != null && _player.RightHand != _player.LeftHand) 
            currentLuck += _player.RightHand.LuckModifier;

        Console.SetCursorPosition(uiColumn, 9);
        Console.Write($"Luck: {currentLuck}  Wisdom: {_player.Wisdom}".PadRight(clearWidth));
        
        Console.SetCursorPosition(uiColumn, 10);
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.Write($"STYL ATAKU: {_player.CurrentAttack.GetType().Name}".PadRight(clearWidth));
        Console.ResetColor();

        Console.SetCursorPosition(uiColumn, 11);
        Enemy target = _dungeon.GetEnemyAt(_player.X, _player.Y);
        
        if (target != null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            string trybInfo = _player.IsInCombatMode ? "[WALKA]" : "[MOZLIWE]";
            Console.Write($"{trybInfo} CEL: {target.Name} (HP: {target.Health} | Atk: {target.BaseDamage})".PadRight(clearWidth));
            Console.ResetColor();
        }
        else 
        {
            Console.Write("".PadRight(clearWidth));
        }
        Console.SetCursorPosition(uiColumn, 13);
        string lh = _player.LeftHand != null ? _player.LeftHand.Name : "Pusta";
        string rh = _player.RightHand != null ? _player.RightHand.Name : "Pusta";
        Console.Write($"LEWA (L): {lh}".PadRight(clearWidth));
        
        Console.SetCursorPosition(uiColumn, 14);
        Console.Write($"PRAWA (R): {rh}".PadRight(clearWidth));

        Console.SetCursorPosition(uiColumn, 16);
        Console.Write("=== SKŁAD (PLECAK) ===".PadRight(clearWidth));
        _player.ClampInventorySelection();

        for (int i = 0; i < Math.Max(10, _player.Backpack.Count); i++)
        {
            Console.SetCursorPosition(uiColumn, i + 17);
            if (i < _player.Backpack.Count)
            {
                string prefix = (i == _player.SelectedInventorySlot) ? "-> " : "   ";
                Console.Write($"{prefix}{_player.Backpack[i].Name}".PadRight(clearWidth));
            }
            else Console.Write("".PadRight(clearWidth));
        }

        Console.SetCursorPosition(0, _dungeon.Height + 2);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write(_statusMessage.PadRight(uiColumn - 2)); 
        Console.ResetColor();
    }
    private void ShowIntro()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Green; 

        if (File.Exists("Intro.txt"))
        {
            string logo = File.ReadAllText("Intro.txt");
            Console.WriteLine(logo);
        }
        else
        {
            Console.WriteLine("Brak pliku Intro.txt! Upewnij się, że jest w folderze z grą (Copy to Output Directory).");
        }
        Console.ResetColor();

        Console.WriteLine("\n\t\t\t [ NACIŚNIJ ENTER, ABY ROZPOCZĄĆ PRZYGODĘ ]");
        
        while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
        Console.Clear();
        
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n\t\tDAWNO TEMU W ODLEGŁEJ GALAKTYCE... A MOŻE PRZY ŁAZIENKOWSKIEJ...\n\n");
        string opowiesc = _factory.GetIntroMessage();
        foreach (char c in opowiesc)
        {
            Console.Write(c);
            if (c != ' ') 
            {
                Thread.Sleep(30); 
            }
            if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Spacebar)
            {
                Console.Clear();
                Console.WriteLine("\n\t\tDAWNO TEMU W ODLEGŁEJ GALAKTYCE... A MOŻE PRZY ŁAZIENKOWSKIEJ...\n\n");
                Console.WriteLine(opowiesc);
                break;
            }
        }
        
        Console.ResetColor();

        Console.WriteLine("\n\nNaciśnij dowolny klawisz, aby zobaczyć sterowanie...");
        Console.ReadKey(true);
        
        Console.Clear();
        Console.WriteLine("=== INSTRUKCJA TAKTYCZNA (STEROWANIE) ===");
        Console.WriteLine("\n[W, A, S, D]      - Poruszanie się po mapie");
        Console.WriteLine("[STRZAŁKI </>]    - Wybieranie przedmiotów na ziemi");
        Console.WriteLine("""[STRZAŁKI /|\ / \|/ ]    - Przeglądanie plecaka""");
        Console.WriteLine("[E]               - Podniesienie przedmiotu");
        Console.WriteLine("[F]               - Wyrzucenie przedmiotu z plecaka");
        Console.WriteLine("[L]               - Wyposażenie w lewą rękę");
        Console.WriteLine("[R]               - Wyposażenie w prawą rękę");
        Console.WriteLine("[1, 2, 3]         - Zmiana stylu walki (Zwykły/Skryty/Magiczny)");
        Console.WriteLine("[X]               - Wejście/Wyjście z trybu walki");
        Console.WriteLine("[Q]               - Poddanie meczu (Wyjście)");

        Console.WriteLine("\n\nWszystko jasne? Ruszajmy na boisko!");
        Console.WriteLine("Naciśnij dowolny klawisz, aby wybiec z tunelu...");
        Console.ReadKey(true);
    }
    private void ShowGameOver()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Red;
        if (File.Exists("end.txt"))
        {
            Console.WriteLine(File.ReadAllText("end.txt"));
        }
        else
        {
            Console.WriteLine("======================================");
            Console.WriteLine("          G A M E   O V E R  " + "         ");
            Console.WriteLine("======================================");
            Console.WriteLine("Twoja przygoda na Legiolandzie dobiegła końca...");
        }

        Console.ResetColor();
        Console.WriteLine("\n--- PODSUMOWANIE SEZONU ---");
        Console.WriteLine($"Zawodnik: {_config.PlayerName}");
        Console.WriteLine($"Punkty: {_player.Points}");
        Console.WriteLine($"Gole: {_player.Goals}");

        Logger.Instance.Log($"KONIEC GRY. Gracz poległ. Punkty: {_player.Points}, Gole: {_player.Goals}");
        Logger.Instance.SaveToFile(_config.PlayerName, _config.LogFilePath);
        Console.WriteLine($"\n[Protokół meczowy (logi) został zapisany w: {_config.LogFilePath}]");
        Console.WriteLine("\nNaciśnij dowolny klawisz, aby wyjść do szatni...");
        Console.ReadKey(true);
    }
}