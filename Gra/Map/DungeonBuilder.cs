using Gra.Decorator;

namespace Gra.Map;

public class DungeonBuilder : IDungeonBuilder
{
    private Dungeon _dungeon;
    private Random _rnd = new Random();
    
    public IDungeonBuilder CreateEmptyDungeon(int width, int height)
    {
        _dungeon = new Dungeon(width, height);
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                _dungeon.Grid[i, j] = new EmptyCell(i, j);
            }
        }
        return this;
    }

    public IDungeonBuilder CreateWallDungeon(int width, int height)
    {
        _dungeon = new Dungeon(width, height);
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                _dungeon.Grid[i, j] = new WallCell(i, j);
            }
        }
        return this;
    }

    public IDungeonBuilder AddCorridors(int lenght)
    {
        int pos_x = 0;
        int pos_y = 0;
        int prev_x = 0;
        int prev_y = 0;
        while (lenght-- > 0)
        {
            prev_x = pos_x;
            prev_y = pos_y;
            int direction = _rnd.Next(0, 100);
            switch (direction)
            {
                case <10:
                    pos_y -= 1;
                    break;
                case >= 10 and < 40 :
                    pos_x += 1;
                    break;
                case >= 40 and < 70:
                    pos_x -= 1;
                    break;
                case >= 70:
                    pos_y += 1;
                    break;
            }
            if (pos_x < 0 || pos_x > _dungeon.Width - 1 || pos_y < 0 || pos_y > _dungeon.Height - 1)
            {
                pos_x = prev_x;
                pos_y = prev_y;
                lenght++;
                continue;
            }
            _dungeon.Grid[pos_x, pos_y] = new EmptyCell(pos_x, pos_y);
        }
        return this;
    }

    public IDungeonBuilder AddItems(int length)
    {
        int number_of_items = _rnd.Next(5, length);

        while (number_of_items > 0)
        {
            int x = _rnd.Next(0, _dungeon.Width);
            int y = _rnd.Next(0, _dungeon.Height);
            if (_dungeon.Grid[x, y].IsPassable())
            {
                Items newItem = null;
                int itemLos = _rnd.Next(0, 4);
                if (itemLos == 0) newItem = new Points(3);
                else if (itemLos == 1) newItem = new Goals(_rnd.Next(0, 4));
                else if (itemLos == 2) newItem = new Junk("Rajovic", 'D');
                else if (itemLos == 3) newItem = new Junk("Rosolek", 'K');

                _dungeon.Grid[x, y] = _dungeon.Grid[x, y].ReceiveItem(newItem);
                number_of_items--;
            }
        }
        return this;
    }

    public IDungeonBuilder AddCentralRoom(int width, int height)
    {
        int start_x = (_dungeon.Width - width) / 2;
        int start_y = (_dungeon.Height - height) / 2;
        
        int left = Math.Max(start_x, 0);
        int right = Math.Min(start_x + width, _dungeon.Width);
        int top = Math.Max(start_y, 0);
        int bottom = Math.Min(start_y + height, _dungeon.Height);

        for (int i = left; i < right; i++)
        {
            for (int j = top; j < bottom; j++)
            {
                _dungeon.Grid[i, j] = new EmptyCell(i, j);
            }
        }
    
        return this;
    }

    public IDungeonBuilder AddRooms()
    {
        int numberOfRooms = _rnd.Next(4, 8); 

        for (int r = 0; r < numberOfRooms; r++)
        {
            int roomWidth = _rnd.Next(4, 9);  
            int roomHeight = _rnd.Next(4, 9); 

            int start_x = _rnd.Next(1, _dungeon.Width - 2); 
            int start_y = _rnd.Next(1, _dungeon.Height - 2);

            int left = Math.Max(start_x, 1);
            int right = Math.Min(start_x + roomWidth, _dungeon.Width - 1);
            int top = Math.Max(start_y, 1);
            int bottom = Math.Min(start_y + roomHeight, _dungeon.Height - 1);
            for (int i = left; i < right; i++)
            {
                for (int j = top; j < bottom; j++)
                {
                    _dungeon.Grid[i, j] = new EmptyCell(i, j);
                }
            }
        }
        return this;
    }
    
    public IDungeonBuilder AddWeapons(int length)
    {
        int weaponsToPlace = length;

        while (weaponsToPlace > 0)
        {
            int x = _rnd.Next(0, _dungeon.Width);
            int y = _rnd.Next(0, _dungeon.Height);

            if (_dungeon.Grid[x, y].IsPassable())
            {
                Items newWeapon = null;
            
                int weaponLos = _rnd.Next(0, 3);
                if (weaponLos == 0) 
                    newWeapon = new MagicWeapon("Josue", 'J', 100, true); 
                else if (weaponLos == 1) 
                    newWeapon = new HeavyWeapon("Odidja-Ofoe", 'O', 120, true);
                else if (weaponLos == 2) 
                    newWeapon = new LightWeapon("Elitim", 'E', 25, false);

                Items finalWeapon = newWeapon;
                if (_rnd.Next(0, 100) <= 20) finalWeapon = new ShapeDecorator(finalWeapon);
                if (_rnd.Next(0, 100) <= 20) finalWeapon = new UnluckyDecorator(finalWeapon);
                _dungeon.Grid[x, y] = _dungeon.Grid[x, y].ReceiveItem(finalWeapon);
                weaponsToPlace--;
            }
        }

        return this; 
    }

    
    public IDungeonBuilder AddEnemies(int count)
    {
        int enemiesToPlace = count;

        while (enemiesToPlace > 0)
        {
            int x = _rnd.Next(0, _dungeon.Width);
            int y = _rnd.Next(0, _dungeon.Height);

            
            if (_dungeon.Grid[x, y].IsPassable() && _dungeon.GetEnemyAt(x, y) == null)
            {
                Enemy newEnemy = null;
                int enemyLos = _rnd.Next(0, 3);

                
                if (enemyLos == 0) newEnemy = new ZlyPudel(x, y);
                else if (enemyLos == 1) newEnemy = new Sedzia(x, y);

                _dungeon.Enemies.Add(newEnemy);
                enemiesToPlace--;
            }
        }

        return this;
    }
    
    public Dungeon GetResult()
    {
        _dungeon.Grid[0, 0] = new EmptyCell(0, 0);
        _dungeon.Grid[0, 1] = new EmptyCell(0, 1);
        _dungeon.Grid[1, 0] = new EmptyCell(1, 0);
        _dungeon.Grid[1, 1] = new EmptyCell(1, 1);
        return _dungeon;
    }
}