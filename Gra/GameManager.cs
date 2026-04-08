using Gra.Map;
using Gra.Movement;

namespace Gra;

public class GameManager
{
    private Dictionary<ConsoleKey, ICommand> _commands;
    private Player _player;
    private Dungeon _dungeon;
    private string _statusMessage = ""; 

    public GameManager(Player player, Dungeon dungeon)
    {
        _player = player;
        _dungeon = dungeon;
        _commands = new Dictionary<ConsoleKey, ICommand>(); 
        SetCommands();
    }
    
    private void SetCommands()
    {
        _commands.Add(ConsoleKey.W,new MoveCommand(_player,_dungeon,0,-1));
        _commands.Add(ConsoleKey.S,new MoveCommand(_player,_dungeon,0,1));
        _commands.Add(ConsoleKey.A,new MoveCommand(_player,_dungeon,-1,0));
        _commands.Add(ConsoleKey.D,new MoveCommand(_player,_dungeon,1,0));
        _commands.Add(ConsoleKey.R,new EquipCommand(_player,true));
        _commands.Add(ConsoleKey.L,new EquipCommand(_player,false));
        _commands.Add(ConsoleKey.UpArrow,new InventoryUpCommand(_player));
        _commands.Add(ConsoleKey.DownArrow,new InventoryDownCommand(_player));
        _commands.Add(ConsoleKey.LeftArrow,new GroundSelectLeftCommand(_player,_dungeon));
        _commands.Add(ConsoleKey.RightArrow,new GroundSelectRightCommand(_player,_dungeon));
        _commands.Add(ConsoleKey.E,new PickUpCommand(_player,_dungeon));
        _commands.Add(ConsoleKey.F,new DropCommand(_player,_dungeon));
    }

    public void StartGame()
    {
        ShowIntro(); 

        Console.Clear();
        Console.CursorVisible = false;

        while (true)
        {
            Draw(); 
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            if (keyInfo.Key == ConsoleKey.Q) break;
            
            // --- NOWE: Zmiana ataku "w locie" bez dedykowanych komend ---
            if (keyInfo.Key == ConsoleKey.D1) { _player.CurrentAttack = new AtakZwyklyVisitor(); _statusMessage = "Styl walki: ZWYKŁY"; continue; }
            if (keyInfo.Key == ConsoleKey.D2) { _player.CurrentAttack = new AtakSkrytyVisitor(); _statusMessage = "Styl walki: SKRYTY"; continue; }
            if (keyInfo.Key == ConsoleKey.D3) { _player.CurrentAttack = new AtakMagicznyVisitor(); _statusMessage = "Styl walki: MAGICZNY"; continue; }

            if (_commands.ContainsKey(keyInfo.Key))
            {
                _commands[keyInfo.Key].Execute();
                _statusMessage = ""; 
            }
            else
            {
                _statusMessage = $"!!! Klawisz {keyInfo.Key} nieaktywny !!!";
            }
        }
    }
    
    private void ShowIntro()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Green; 

        string logo = @"
 /$$      /$$           /$$                                                   /$$$$$$$$       
| $$  /$ | $$          | $$                                                  |__  $$__/       
| $$ /$$$| $$  /$$$$$$ | $$  /$$$$$$$  /$$$$$$  /$$$$$$/$$$$   /$$$$$$          | $$  /$$$$$$ 
| $$/$$ $$ $$ /$$__  $$| $$ /$$_____/ /$$__  $$| $$_  $$_  $$ /$$__  $$         | $$ /$$__  $$
| $$$$_  $$$$| $$$$$$$$| $$| $$      | $$  \ $$| $$ \ $$ \ $$| $$$$$$$$         | $$| $$  \ $$
| $$$/ \  $$$| $$_____/| $$| $$      | $$  | $$| $$ | $$ | $$| $$_____/         | $$| $$  | $$
| $$/   \  $$|  $$$$$$$| $$|  $$$$$$$|  $$$$$$/| $$ | $$ | $$|  $$$$$$$         | $$|  $$$$$$/
|__/     \__/ \_______/|__/ \_______/ \______/ |__/ |__/ |__/ \_______/         |__/ \______/ 
    ";
        Console.WriteLine(logo);
        Console.ResetColor();

        Console.WriteLine("\n\t\t\t [ NACIŚNIJ ENTER, ABY ROZPOCZĄĆ PRZYGODĘ ]");
        
        while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
        Console.Clear();
        
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n\t\tDAWNO TEMU W ODLEGŁEJ GALAKTYCE... A MOŻE PRZY ŁAZIENKOWSKIEJ...\n");

        string historia = @"
                EPIZOD I: POWRÓT KLUBU
    
         Wielka trwoga i zamęt opanowały zacne włości Legiolandu! 
        Oto bowiem Zły Pudel, kudłaty tyran o sercu z kamienia, 
        ład boiskowy zburzył i mrok na trybuny sprowadził. 
        
        Azaliż znajdzie się śmiałek, co godność herbu uratuje? 
        Oto Ty, zacny Waszmość-Graczu, stajesz przed wyzwaniem!
        Twoim zadaniem jest punkty i gole gromadzić, 
        by chwałę dawną krainie owej przywrócić.
        
        W labiryntach owych skarby bezcenne odnajdziesz: 
        mędrca Josue, walecznego Odidję-Ofoe oraz Elitima chyżego. 
        Lecz strzeż się, mości panie, śmieci pospolitych! 
        Rajovic i Rosołek niczym chwasty pod nogami 
        plątać się będą, jeno miejsce w taborze marnując.
        
        Ruszaj tedy! Niechaj Legia będzie z Tobą!
        ";

        foreach (char c in historia)
        {
            Console.Write(c); 
        }
        
        Console.WriteLine("\nNaciśnij dowolny klawisz, aby zobaczyć sterowanie...");
        Console.ReadKey(true);
        
        Console.Clear();
        Console.WriteLine("=== INSTRUKCJA TAKTYCZNA (STEROWANIE) ===");
        Console.WriteLine("\n[W, A, S, D]      - Poruszanie się po mapie");
        Console.WriteLine("[STRZAŁKI </>]    - Wybieranie przedmiotów na ziemi");
        Console.WriteLine("""[STRZAŁKI /|\ / \|/ ]    - Przeglądanie plecaka""");
        Console.WriteLine("[E]               - Podniesienie przedmiotu");
        Console.WriteLine("[T]               - Wyrzucenie przedmiotu z plecaka");
        Console.WriteLine("[L]               - Wyposażenie w lewą rękę");
        Console.WriteLine("[R]               - Wyposażenie w prawą rękę");
        Console.WriteLine("[1, 2, 3]         - Zmiana stylu walki (Zwykły/Skryty/Magiczny)");
        Console.WriteLine("[Q]               - Poddanie meczu (Wyjście)");

        Console.WriteLine("\n\nWszystko jasne? Ruszajmy na boisko!");
        Console.WriteLine("Naciśnij dowolny klawisz, aby wybiec z tunelu...");
        Console.ReadKey(true);
    }
    
    private void Draw()
    {
        Console.SetCursorPosition(0, 0);
        
        _dungeon.Draw(_player);

        int uiColumn = _dungeon.Width  + 20; 
        int clearWidth = 60; 

        Console.SetCursorPosition(uiColumn, 0);
        // Dodany LEGIOLAND w tytule!
        Console.Write("=== LEGIOLAND (MAPA) ===".PadRight(clearWidth));
        
        var itemsOnGround = _dungeon.Grid[_player.X, _player.Y].GetItemNames();
        _player.ClampGroundSelection(itemsOnGround.Count);
        int end_listing = Math.Max(itemsOnGround.Count, 7);
        
        for (int i = 0; i < Math.Max(5, itemsOnGround.Count); i++)
        {
            Console.SetCursorPosition(uiColumn, i + 1);
            if (i < itemsOnGround.Count)
            {
                string prefix = (i == _player.SelectedGroundSlot) ? "-> " : "   ";
                Console.Write($"{prefix}{itemsOnGround[i]} [Press E to equip]".PadRight(30));
            }
            else Console.Write("".PadRight(30)); 
        }

        Console.SetCursorPosition(uiColumn, end_listing);
        Console.Write($"PUNKTY: {_player.Points}  GOLE: {_player.Goals}".PadRight(30));
        
        Console.SetCursorPosition(uiColumn, 8);
        Console.Write($"HP: {_player.Health}   STRENGHT : {_player.Strength}".PadRight(30));
        
        int currentLuck = _player.Luck;
        if (_player.LeftHand != null) currentLuck += _player.LeftHand.LuckModifier;
        if (_player.RightHand != null && _player.RightHand != _player.LeftHand) 
            currentLuck += _player.RightHand.LuckModifier;

        Console.SetCursorPosition(uiColumn, 9);
        Console.Write($"Luck: {currentLuck}  Wisdom: {_player.Wisdom}".PadRight(30));
        
        Console.SetCursorPosition(uiColumn, 10);
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.Write($"STYL WALKI: {_player.CurrentAttack.GetType().Name}".PadRight(30));
        Console.ResetColor();

        Console.SetCursorPosition(uiColumn, 12);
        string lh = _player.LeftHand != null ? _player.LeftHand.Name : "Pusta";
        string rh = _player.RightHand != null ? _player.RightHand.Name : "Pusta";
        Console.Write($"LEWA: {lh}".PadRight(30));
        
        Console.SetCursorPosition(uiColumn, 13);
        Console.Write($"PRAWA: {rh}".PadRight(30));

        Console.SetCursorPosition(uiColumn, 15);
        Console.Write("=== SKŁAD (PLECAK) ===".PadRight(30));
        _player.ClampInventorySelection();

        for (int i = 0; i < Math.Max(10, _player.Backpack.Count); i++)
        {
            Console.SetCursorPosition(uiColumn, i + 16);
            if (i < _player.Backpack.Count)
            {
                string prefix = (i == _player.SelectedInventorySlot) ? "-> " : "   ";
                Console.Write($"{prefix}{_player.Backpack[i].Name}".PadRight(30));
            }
            else Console.Write("".PadRight(30));
        }

        Console.SetCursorPosition(0, _dungeon.Height + 2);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write(_statusMessage.PadRight(30)); 
        Console.ResetColor();
    }
}