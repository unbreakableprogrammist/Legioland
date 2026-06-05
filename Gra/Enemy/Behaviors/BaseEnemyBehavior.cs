using Gra.Map;

namespace Gra.Behaviors;

public abstract class BaseEnemyBehavior : IEnemyBehavior
{
    protected Random _rnd = new Random();

    public abstract void ExecuteTurn(Enemy enemy, Dungeon dungeon, Dictionary<int, Player> players);
    // to ze potwor widzi gracza zakladamy ze jest wtedy gdy jest z nim w lini prostej 
    protected Player GetClosestVisiblePlayer(Enemy enemy, Dungeon dungeon, Dictionary<int, Player> players)
    {
        Player closestPlayer = null;
        int minDistance = int.MaxValue;

        foreach (var player in players.Values)
        {
            if (player.Health <= 0) continue; 
            if (player.X == enemy.X || player.Y == enemy.Y)
            {
                int dist = Math.Abs(player.X - enemy.X) + Math.Abs(player.Y - enemy.Y);
                if (dist < minDistance && HasLineOfSight(enemy.X, enemy.Y, player.X, player.Y, dungeon))
                {
                    minDistance = dist;
                    closestPlayer = player;
                }
            }
        }
        return closestPlayer;
    }
    // sprawdza czy nie ma miedzy nimi sciany 
    private bool HasLineOfSight(int startX, int startY, int targetX, int targetY, Dungeon dungeon)
    {
        int stepX = Math.Sign(targetX - startX);
        int stepY = Math.Sign(targetY - startY);

        int currX = startX + stepX;
        int currY = startY + stepY;

        while (currX != targetX || currY != targetY)
        {
            if (!dungeon.Grid[currX, currY].IsPassable()) return false; 
            currX += stepX;
            currY += stepY;
        }
        return true;
    }

    // porusza sie w strone celu 
    protected void MoveTowards(Enemy enemy, Dungeon dungeon, int targetX, int targetY)
    {
        int stepX = Math.Sign(targetX - enemy.X);
        int stepY = Math.Sign(targetY - enemy.Y);

        if (stepX != 0 && dungeon.Grid[enemy.X + stepX, enemy.Y].IsPassable())
        {
            enemy.X += stepX;
        }
        else if (stepY != 0 && dungeon.Grid[enemy.X, enemy.Y + stepY].IsPassable())
        {
            enemy.Y += stepY;
        }
    }

    // --- udcieka od gracza 
    protected bool MoveAwayFrom(Enemy enemy, Dungeon dungeon, int targetX, int targetY)
    {
        int stepX = Math.Sign(enemy.X - targetX); 
        int stepY = Math.Sign(enemy.Y - targetY);

        if (stepX != 0 && enemy.X + stepX >= 0 && enemy.X + stepX < dungeon.Width && dungeon.Grid[enemy.X + stepX, enemy.Y].IsPassable())
        {
            enemy.X += stepX; return true;
        }
        if (stepY != 0 && enemy.Y + stepY >= 0 && enemy.Y + stepY < dungeon.Height && dungeon.Grid[enemy.X, enemy.Y + stepY].IsPassable())
        {
            enemy.Y += stepY; return true;
        }
        
        // probujemy uciekac na bok 
        int[] altDx = { 0, 0, 1, -1 };
        int[] altDy = { 1, -1, 0, 0 };
        for (int i = 0; i < 4; i++)
        {
            int nx = enemy.X + altDx[i];
            int ny = enemy.Y + altDy[i];
            if (nx >= 0 && nx < dungeon.Width && ny >= 0 && ny < dungeon.Height && dungeon.Grid[nx, ny].IsPassable())
            {
                enemy.X = nx; enemy.Y = ny; return true;
            }
        }
        return false; // Nie ma ucieczki
    }
    
    protected void MoveRandomly(Enemy enemy, Dungeon dungeon)
    {
        int[] dx = { 0, 0, 1, -1 };
        int[] dy = { 1, -1, 0, 0 };
        int direction = _rnd.Next(4);
        int newX = enemy.X + dx[direction];
        int newY = enemy.Y + dy[direction];

        if (newX >= 0 && newX < dungeon.Width && newY >= 0 && newY < dungeon.Height && dungeon.Grid[newX, newY].IsPassable())
        {
            enemy.X = newX;
            enemy.Y = newY;
        }
    }
}