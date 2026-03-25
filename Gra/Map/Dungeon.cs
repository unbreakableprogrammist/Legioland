namespace Gra.Map;

/// <summary>
/// Ta klasa implementuje obiekt ktory bedzie mapa 
/// </summary>

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
        // Ustawiamy kursor na początku, żeby nadpisywać starą mapę (unikamy migotania)
        Console.SetCursorPosition(0, 0);

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                // SPRAWDZAMY: Czy na tym polu stoi gracz?
                if (x == player.X && y == player.Y)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan; // Kolor gracza
                    Console.Write("P"); // Symbol gracza (możesz zmienić na swój)
                    Console.ResetColor();
                }
                else
                {
                    // Jeśli nie gracz, to rysujemy to, co siedzi w komórce
                    char symbol = Grid[x, y].GetSymbol();
                
                    // Opcjonalnie: kolory dla różnych typów (ściana vs trawa)
                    if (symbol == '#') Console.ForegroundColor = ConsoleColor.DarkGray;
                
                    Console.Write($"{symbol}");
                    Console.ResetColor();
                }
            }
            // Po narysowaniu całego rzędu X, przechodzimy do nowej linii
            Console.WriteLine();
        }
    }
}