namespace Gra.Map;

public interface IDungeonBuilder
{
    IDungeonBuilder CreateEmptyDungeon(int width, int height); // tworzy pusty loch
    IDungeonBuilder CreateWallDungeon(int width, int height); // tworzy same sciany 
    
    IDungeonBuilder AddCorridors(int lenght);
    IDungeonBuilder AddRooms();
    IDungeonBuilder AddCentralRoom(int width, int height);
    IDungeonBuilder AddItems(int lenght);
    IDungeonBuilder AddWeapons(int lenght);

    Dungeon GetResult();
}