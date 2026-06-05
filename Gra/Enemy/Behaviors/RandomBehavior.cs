namespace Gra.Behaviors;

using Gra.Map;
using System.Collections.Generic;

public class RandomBehavior : BaseEnemyBehavior
{
    public override void ExecuteTurn(Enemy enemy, Dungeon dungeon, Dictionary<int, Player> players)
    {
        if (enemy.IsDead) return;
        MoveRandomly(enemy, dungeon); 
    }
}