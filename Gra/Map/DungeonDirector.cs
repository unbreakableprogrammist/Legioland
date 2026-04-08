namespace Gra.Map
{
    public class DungeonDirector 
    {
        // Główny, skomplikowany loch z pokojami i korytarzami
        public Dungeon BuildLegioland(IDungeonBuilder builder, int width, int height)
        {
            return builder
                .CreateWallDungeon(width, height) 
                .AddCentralRoom(10, 6)             
                .AddRooms()                        
                .AddCorridors(100)                 
                .AddItems(15)                      
                .AddWeapons(4)
                .AddEnemies(6) // Dodajemy wrogów do głównego lochu                     
                .GetResult();                      
        }

        // Proste boisko treningowe z bronią
        public Dungeon BuildTrainingPitch(IDungeonBuilder builder, int width, int height)
        {
            return builder
                .CreateEmptyDungeon(width, height)
                .AddWeapons(2)
                .GetResult();
        }

        // --- NOWA METODA: PUSTA ARENA Z WROGAMI ---
        public Dungeon BuildBattleArena(IDungeonBuilder builder, int width, int height, int enemyCount)
        {
            return builder
                .CreateEmptyDungeon(width, height) // Pusta arena bez ścian w środku
                .AddEnemies(enemyCount)            // Wrzucamy zadaną liczbę wrogów
                .GetResult();
        }
    }
}