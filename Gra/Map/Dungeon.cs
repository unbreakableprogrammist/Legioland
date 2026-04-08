namespace Gra.Map;

public class Dungeon
{
    public Cell[,] Grid { get; private set; }
    public int Width { get; private set; }
    public int Height { get; private set; }
    
    // Zmieniamy na 'private set', żeby nikt z zewnątrz nie mógł napisać dungeon.Enemies = null;
    public List<Enemy> Enemies { get; private set; } = new List<Enemy>();

    public Dungeon(int width, int height)
    {
        Width = width;
        Height = height;
        Grid = new Cell[width, height];
        // Inicjalizacja listy w konstruktorze - podwójne zabezpieczenie przed NullReference
        Enemies = new List<Enemy>();
    }
    
    public bool CanEnter(int x, int y)
    {
        if (x < 0 || x >= Width || y < 0 || y >= Height) return false;
        return Grid[x, y].IsPassable();
    }
    
    public void Draw(Player player)
    {
        Console.SetCursorPosition(0, 0);

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                // 1. PRIORYTET: Gracz
                if (x == player.X && y == player.Y)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("P");
                    Console.ResetColor();
                }
                else
                {
                    // 2. PRIORYTET: Wróg
                    Enemy enemy = GetEnemyAt(x, y);
                    if (enemy != null)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(enemy.Symbol); // Rysujemy symbol konkretnego wroga (np. 'S', 'K')
                        Console.ResetColor();
                    }
                    else
                    {
                        // 3. PRIORYTET: Komórka mapy
                        char symbol = Grid[x, y].GetSymbol();
                        if (symbol == '#') Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(symbol);
                        Console.ResetColor();
                    }
                }
            }
            Console.WriteLine();
        }
    }

    public Enemy GetEnemyAt(int x, int y)
    {
        // Programowanie defensywne: jeśli lista jakimś cudem jest nullem, nie syp błędem
        if (Enemies == null) return null;

        foreach (var enemy in Enemies)
        {
            // Sprawdzamy tylko żywych wrogów na konkretnych pozycjach
            if (enemy != null && !enemy.IsDead && enemy.X == x && enemy.Y == y)
            {
                return enemy;
            }
        }
        return null;
    }
}