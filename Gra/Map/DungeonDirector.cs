namespace Gra.Map
{
    public class DungeonDirector 
    {
        public Dungeon BuildLegioland(IDungeonBuilder builder, int width, int height)
        {
            return builder
                .CreateWallDungeon(width, height) 
                .AddCentralRoom(10, 6)             
                .AddRooms()                        
                .AddCorridors(100)                 
                .AddItems(15)                      
                .AddWeapons(4)                     
                .GetResult();                      
        }

        public Dungeon BuildTrainingPitch(IDungeonBuilder builder, int width, int height)
        {
            return builder
                .CreateEmptyDungeon(width, height)
                .AddWeapons(2)
                .GetResult();
        }
    }
}