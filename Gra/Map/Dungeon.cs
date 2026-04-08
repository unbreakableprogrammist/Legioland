namespace Gra.Map;

public class Dungeon
{
    public Cell[,] Grid { get; private set;} // dostepny dla wszytskich mapa 
    public int Width { get; private set; }
    public int Height { get; private set; }

    public Dungeon(int width, int height) // konstruktor 
    {
        Width = width;
        Height = height;
        Grid = new Cell[width, height];
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
                if (x == player.X && y == player.Y)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan; // Kolor gracza
                    Console.Write("P");
                    Console.ResetColor();
                }
                else
                {
                    char symbol = Grid[x, y].GetSymbol();
                
                    if (symbol == '#') Console.ForegroundColor = ConsoleColor.DarkGray;
                
                    Console.Write($"{symbol}");
                    Console.ResetColor();
                }
            }
            Console.WriteLine();
        }
    }
}