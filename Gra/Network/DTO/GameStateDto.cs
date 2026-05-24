namespace Gra.Network.DTO;

// obiekt do ktorego beda pakowane dane do wyslania do klienta zeby ConsoleView narysowal 
// Główny obiekt przesyłany z Serwera do Klientów
public class GameStateDto
{
    public List<PlayerDto> Players { get; set; } = new List<PlayerDto>();
    public List<EnemyDto> Enemies { get; set; } = new List<EnemyDto>();
    public CellDto[][] Map { get; set; }
    public List<string> Logs { get; set; } = new List<string>();
        
    public int MapWidth { get; set; }
    public int MapHeight { get; set; }
}

public class PlayerDto
{
    public int Id { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public char Symbol { get; set; } 
        
    // Statystyki do UI
    public int Health { get; set; }
    public int Points { get; set; }
    public int Goals { get; set; }
    public int Strength { get; set; }
    public int TotalLuck { get; set; }
    public int Wisdom { get; set; }
        
    // Ekwipunek i status jako stringi (bo widok potrzebuje tylko tekstu)
    public string AttackStyleName { get; set; }
    public string LeftHandName { get; set; }
    public string RightHandName { get; set; }
    public List<string> BackpackNames { get; set; } = new List<string>();
        
    // Sterowanie UI
    public bool IsInCombatMode { get; set; }
    public int SelectedInventorySlot { get; set; }
    public int SelectedGroundSlot { get; set; }
}

public class EnemyDto
{
    public int X { get; set; }
    public int Y { get; set; }
    public char Symbol { get; set; }
    public string Name { get; set; }
    public int Health { get; set; }
    public int BaseDamage { get; set; }
}

public class CellDto
{
    public char Symbol { get; set; }
    public List<string> Items { get; set; } = new List<string>();
}