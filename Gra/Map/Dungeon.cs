namespace Gra.Map;

public class Dungeon
{
    public Cell[,] Grid { get; private set; }
    public int Width { get; private set; }
    public int Height { get; private set; }
    
    
    public List<Enemy> Enemies { get; private set; } = new List<Enemy>();

    public Dungeon(int width, int height)
    {
        Width = width;
        Height = height;
        Grid = new Cell[width, height];
        
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
                
                if (x == player.X && y == player.Y)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("P");
                    Console.ResetColor();
                }
                else
                {
                    
                    Enemy enemy = GetEnemyAt(x, y);
                    if (enemy != null)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(enemy.Symbol); 
                        Console.ResetColor();
                    }
                    else
                    {
                        
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
        
        if (Enemies == null) return null;

        foreach (var enemy in Enemies)
        {
            
            if (enemy != null && !enemy.IsDead && enemy.X == x && enemy.Y == y)
            {
                return enemy;
            }
        }
        return null;
    }
    
    public int CalculatePathDistance(int startX, int startY, int targetX, int targetY, int maxRange)
    {
        if (startX == targetX && startY == targetY) return 0;

        var queue = new Queue<(int x, int y, int dist)>();
        var visited = new HashSet<(int, int)>();

        queue.Enqueue((startX, startY, 0));
        visited.Add((startX, startY));

        int[] dx = { 0, 0, -1, 1 };
        int[] dy = { -1, 1, 0, 0 };

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            if (current.x == targetX && current.y == targetY)
                return current.dist;

            if (current.dist >= maxRange)
                continue;

            for (int i = 0; i < 4; i++)
            {
                int nx = current.x + dx[i];
                int ny = current.y + dy[i];
                if (nx >= 0 && nx < Width && ny >= 0 && ny < Height && !visited.Contains((nx, ny)) &&  Grid[nx, ny].IsPassable())
                {
                    visited.Add((nx, ny));
                    queue.Enqueue((nx, ny, current.dist + 1));
                }
            }
        }
        return -1; // Nie znaleziono drogi w podanym zasięgu
    }
}