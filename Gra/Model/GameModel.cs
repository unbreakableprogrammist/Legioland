using Gra.Logging;
using Gra.Map;
using Gra.Network.DTO;
using Gra.Observer;

namespace Gra.Model;

public class GameModel
{
    public Dungeon Dungeon { get; set; }
    public Dictionary<int, Player> Players { get; set; } = new Dictionary<int, Player>();
    public ISubject<SoundPayload> SoundNetwork { get; private set; }
    public GameModel(Dungeon dungeon, ISubject<SoundPayload> soundNetwork)
    {
        Dungeon = dungeon;
        SoundNetwork = soundNetwork;
        Players = new Dictionary<int, Player>();
    }
    public void AddPlayer(int playerId, Player player)
    {
        if (!Players.ContainsKey(playerId))
        {
            Players.Add(playerId, player);
        }
    }

    public void RemovePlayer(int playerId)
    {
        if (Players.ContainsKey(playerId))
        {
            Players.Remove(playerId);
        }
    }
    public void MoveAllEnemies()
    {
        foreach (var enemy in Dungeon.Enemies)
        {
            if (!enemy.IsDead) enemy.Move();
        }
    }

    public List<string> GetLogs()
    {
        return Logger.Instance.GetLogs();
    }
    public GameStateDto ToDto()
    {
        var dto = new GameStateDto
        {
            MapWidth = Dungeon.Width,
            MapHeight = Dungeon.Height,
            Logs = GetLogs()
        };

        foreach (var kvp in Players)
        {
            Player p = kvp.Value;
            
            int currentLuck = p.Luck;
            if (p.LeftHand != null) currentLuck += p.LeftHand.LuckModifier;
            if (p.RightHand != null && p.RightHand != p.LeftHand) currentLuck += p.RightHand.LuckModifier;

            dto.Players.Add(new PlayerDto
            {
                Id = kvp.Key,
                X = p.X,
                Y = p.Y,
                Symbol = kvp.Key.ToString()[0],
                Health = p.Health,
                Points = p.Points,
                Goals = p.Goals,
                StatusMessage = p.StatusMessage,
                Strength = p.Strength,
                TotalLuck = currentLuck,
                Wisdom = p.Wisdom,
                AttackStyleName = p.CurrentAttack.GetType().Name,
                LeftHandName = p.LeftHand?.Name ?? "Pusta",
                RightHandName = p.RightHand?.Name ?? "Pusta",
                BackpackNames = p.Backpack.Select(i => i.Name).ToList(),
                IsInCombatMode = p.IsInCombatMode,
                SelectedInventorySlot = p.SelectedInventorySlot,
                SelectedGroundSlot = p.SelectedGroundSlot
            });
        }

        foreach (var e in Dungeon.Enemies)
        {
            if (!e.IsDead)
            {
                dto.Enemies.Add(new EnemyDto 
                { 
                    X = e.X, 
                    Y = e.Y, 
                    Symbol = e.Symbol, 
                    Name = e.Name, 
                    Health = e.Health, 
                    BaseDamage = e.BaseDamage 
                });
            }
        }

        dto.Map = new CellDto[Dungeon.Width][];
        for (int x = 0; x < Dungeon.Width; x++)
        {
            dto.Map[x] = new CellDto[Dungeon.Height];
            for (int y = 0; y < Dungeon.Height; y++)
            {
                dto.Map[x][y] = new CellDto
                {
                    Symbol = Dungeon.Grid[x, y].GetSymbol(),
                    Items = Dungeon.Grid[x, y].GetItemNames()
                };
            }
        }

        return dto;
    }
}
