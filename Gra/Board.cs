namespace Gra;

public class Board
{
    private Cell[,] _grid;
    private readonly int _width;
    private readonly int _height;
    private Player _gracz = new Player(0,0);

    public Board(int width, int height)
    {
        _width = width;
        _height = height;
        _grid = new Cell[width, height];
        Random rnd = new Random(); 
        // generowanie planszy 
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int los = rnd.Next() % 100; 
                if (los < 10) // 10% szans na ściane 
                {
                    _grid[x, y] = new WallCell(x, y); 
                }
                else if (los >= 10 && los < 20) // 10% szans na przedmioty
                {
                    ItemCell cellWithItem = new ItemCell(x, y);
            
                    // Losujemy, co leży na tym polu
                    int itemLos = rnd.Next() % 8;
                    if (itemLos == 0) cellWithItem.AddItem(new Coin(15));
                    else if (itemLos == 1) cellWithItem.AddItem(new Gold(5));
                    else if (itemLos == 2) cellWithItem.AddItem(new Junk("Kielich", 'U'));
                    else if (itemLos == 3) cellWithItem.AddItem(new Junk("Stara Ksiazka", 'B'));
                    else if (itemLos == 4) cellWithItem.AddItem(new Weapon("Krotki Miecz", '/', 5, false));
                    else if (itemLos == 5) cellWithItem.AddItem(new Weapon("Wielki Topor", 'T', 15, true)); 
                    _grid[x, y] = cellWithItem;
                }
                else // Puste pole
                {
                    _grid[x, y] = new EmptyCell(x, y); 
                }
            }
        }
        _grid[0, 0] = new EmptyCell(0, 0); // start musi byc pusty 
    }
    
    public void Draw()
    {
        Console.SetCursorPosition(0, 0); // stawiamy kursor w punkcie 0,0

        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                if (x == _gracz.X && y == _gracz.Y)
                    Console.Write("[¶]");
                else
                    Console.Write($"[{_grid[x, y].GetSymbol()}]"); 
            }
            Console.WriteLine();
        }

        int uiColumn = _width * 3 + 4; // Ustawienie interfejsu obok planszy , poniewaz rysujemy [pole] to mamy 3 razy wiecej znakow

        // --- SEKCJA 1: INFO ---
        Console.SetCursorPosition(uiColumn, 0); // Zaczynamy od samej góry
        Console.Write("=== INFORMACJE O POLU ===");

        List<string> itemsOnGround = _grid[_gracz.X, _gracz.Y].GetItemNames();

        Console.SetCursorPosition(uiColumn, 1); // zapobiega migotaniu (ladnie przenosi cursor)
        if (itemsOnGround.Count > 0) // jesli na tym polu sa jakies itemki 
        {
            Console.Write("Widzisz pod nogami:                 "); 
            int row = 2;
            foreach(var itemName in itemsOnGround)
            {
                Console.SetCursorPosition(uiColumn, row++);
                Console.Write("- " + itemName + "                      ");
            }
            for(int i = row; i < 5; i++) // tutaj troszke zalozenie ze nie ma na razie wiecej niz 4 przedmiotow na polu 
            {
                Console.SetCursorPosition(uiColumn, i);
                Console.Write("                                    ");
            }
        }
        else // puste pole
        {
            Console.Write("Stoisz na pustym polu.              ");
            for(int i = 2; i < 5; i++)
            {
                Console.SetCursorPosition(uiColumn, i);
                Console.Write("                                    ");
            }
        }

        // --- SEKCJA 2: BOGACTWO ---
        Console.SetCursorPosition(uiColumn, 6);
        Console.Write("=== BOGACTWO ===");
        
        Console.SetCursorPosition(uiColumn, 7);
        Console.Write($"Monety: {_gracz.Coins}       "); 
        Console.SetCursorPosition(uiColumn, 8);
        Console.Write($"Zloto:  {_gracz.Gold}       ");

        // --- SEKCJA 3: ATRYBUTY ---
        Console.SetCursorPosition(uiColumn, 10);
        Console.Write("=== ATRYBUTY ===");
        
        Console.SetCursorPosition(uiColumn, 11);
        Console.Write($"Zdrowie:   {_gracz.Health}      ");
        Console.SetCursorPosition(uiColumn, 12);
        Console.Write($"Sila:      {_gracz.Strength}      ");
        Console.SetCursorPosition(uiColumn, 13);
        Console.Write($"Zrecznosc: {_gracz.Dexterity}      ");
        Console.SetCursorPosition(uiColumn, 14);
        Console.Write($"Szczescie: {_gracz.Luck}      ");
        Console.SetCursorPosition(uiColumn, 15);
        Console.Write($"Agresja:   {_gracz.Aggression}      ");
        Console.SetCursorPosition(uiColumn, 16);
        Console.Write($"Madrosc:   {_gracz.Wisdom}      ");
        // --- SEKCJA 4: EKWIPUNEK ---
        Console.SetCursorPosition(uiColumn, 18);
        Console.Write("=== EKWIPUNEK ===");

        // Lewa ręka
        Console.SetCursorPosition(uiColumn, 19);
        string leftHand = _gracz.LeftHand != null ? _gracz.LeftHand.Name : "Pusta";
        Console.Write($"Lewa reka:  {leftHand}                    ");

        // Prawa ręka
        Console.SetCursorPosition(uiColumn, 20);
        string rightHand = _gracz.RightHand != null ? _gracz.RightHand.Name : "Pusta";
        Console.Write($"Prawa reka: {rightHand}                    ");

        // Zawartość plecaka
        Console.SetCursorPosition(uiColumn, 22);
        Console.Write("--- Plecak ---");
        for (int i = 0; i < 10; i++) // wyswietlamy 10 pierwszych
        {
            Console.SetCursorPosition(uiColumn, 23 + i);
            if (i < _gracz.Backpack.Count)
                Console.Write($"{i + 1}. {_gracz.Backpack[i].Name}                    ");
            else
                Console.Write("                                          "); // Czyszczenie pustych miejsc
        }
    }
        
        public bool CanEnter(int x, int y)
        {
            if (x < 0 || x >= _grid.GetLength(0) || y < 0 || y >= _grid.GetLength(1)) // sprawdzamy czy to pole jest tablica
            {
                return false;
            }

            // Jeśli przeszliśmy test wyżej, to na 100% jesteśmy wewnątrz planszy
            return _grid[x, y].IsPassable();
        }

    public void start_game()
    {
        Console.Clear();
        Console.CursorVisible = false; // Ukrywamy kursor

        while (true) // Nieskończona pętla gry
        {
            Draw(); // Rysujemy aktualny stan

            // Czekamy na wciśnięcie klawisza (true ukrywa wciśnięty znak w konsoli)
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);

            // Reakcja na klawisze
            switch (keyInfo.Key)
            {
                case ConsoleKey.W: 
                    _gracz.Move(0, -1, this); // Ruch w górę (Y maleje)
                    break;
                case ConsoleKey.S: 
                    _gracz.Move(0, 1, this);  // Ruch w dół (Y rośnie)
                    break;
                case ConsoleKey.A: 
                    _gracz.Move(-1, 0, this); // Ruch w lewo (X maleje)
                    break;
                case ConsoleKey.D: 
                    _gracz.Move(1, 0, this);  // Ruch w prawo (X rośnie)
                    break;
                case ConsoleKey.E:
                    Items podniesiony = _grid[_gracz.X, _gracz.Y].TakeItem();
                    if (podniesiony != null)
                    {
                        podniesiony.PickUp(_gracz);
                    }
                    break;
                case ConsoleKey.D1: 
                    if (_gracz.Backpack.Count >= 1) _gracz.Backpack[0].Equip(_gracz);
                    break;
                
                // Zakładanie przedmiotu nr 2 z plecaka
                case ConsoleKey.D2: 
                    if (_gracz.Backpack.Count >= 2) _gracz.Backpack[1].Equip(_gracz);
                    break;

                // Zakładanie przedmiotu nr 3 z plecaka
                case ConsoleKey.D3: 
                    if (_gracz.Backpack.Count >= 3) _gracz.Backpack[2].Equip(_gracz);
                    break;
                case ConsoleKey.D4: 
                    if (_gracz.Backpack.Count >= 4) _gracz.Backpack[3].Equip(_gracz);
                    break;
                case ConsoleKey.D5:
                    if(_gracz.Backpack.Count >= 5)  _gracz.Backpack[4].Equip(_gracz);
                    break;
                case ConsoleKey.D6:
                    if(_gracz.Backpack.Count >= 6) _gracz.Backpack[5].Equip(_gracz);
                    break;
                case ConsoleKey.D7:
                    if(_gracz.Backpack.Count >= 7) _gracz.Backpack[6].Equip(_gracz);
                    break;
                case ConsoleKey.D8:
                    if(_gracz.Backpack.Count >= 8) _gracz.Backpack[7].Equip(_gracz);
                    break;
                case ConsoleKey.D9:
                    if(_gracz.Backpack.Count >= 9) _gracz.Backpack[8].Equip(_gracz);
                    break;
                case ConsoleKey.D0:
                    if(_gracz.Backpack.Count >= 10) _gracz.Backpack[9].Equip(_gracz);
                    break;
                // Klawisz R - Rzucanie (Drop) pierwszego przedmiotu z plecaka
                case ConsoleKey.R:
                    if (_gracz.Backpack.Count > 0)
                    {
                        // 1. Wyciągamy pierwszy przedmiot z plecaka
                        Items rzucanyPrzedmiot = _gracz.Backpack[0];
                        _gracz.Backpack.RemoveAt(0); // gracz ogranie usuwanie swojego przedmiotu
                        _grid[_gracz.X, _gracz.Y] = _grid[_gracz.X, _gracz.Y].ReceiveItem(rzucanyPrzedmiot); // nadpisujemy nasze pole nowa komorka (typu ItemCell)
                    }
                    break;
                case ConsoleKey.Q: 
                    return; 
            }
        }
    }
    
}