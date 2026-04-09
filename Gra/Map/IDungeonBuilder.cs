namespace Gra.Map;

public interface IDungeonBuilder
{
    IDungeonBuilder CreateEmptyDungeon(int width, int height); 
    IDungeonBuilder CreateWallDungeon(int width, int height); 
    
    IDungeonBuilder AddCorridors(int lenght);
    IDungeonBuilder AddRooms();
    IDungeonBuilder AddCentralRoom(int width, int height);
    IDungeonBuilder AddItems(int lenght);
    IDungeonBuilder AddWeapons(int lenght);
    IDungeonBuilder AddEnemies(int lenght); 

    Dungeon GetResult();
}