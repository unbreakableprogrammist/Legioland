using Gra.Map;

namespace Gra.Behaviors;

public interface IEnemyBehavior
{
    void ExecuteTurn(Enemy enemy, Dungeon dungeon, Dictionary<int, Player> players);
}