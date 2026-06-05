namespace Gra.Behaviors;

using Gra.Logging;
using Gra.Map;
using System.Collections.Generic;

public class CowardlyBehavior : BaseEnemyBehavior
{
    public override void ExecuteTurn(Enemy enemy, Dungeon dungeon, Dictionary<int, Player> players)
    {
        if (enemy.IsDead) return;
        Player targetPlayer = GetClosestVisiblePlayer(enemy, dungeon, players);

        if (targetPlayer != null)
        {
            enemy.LastHeardSound = null; 
            bool moved = MoveAwayFrom(enemy, dungeon, targetPlayer.X, targetPlayer.Y);
            
            if (moved) Logger.Instance.Log($"[STRACH] {enemy.Name} ucieka przed graczem!");
            else MoveRandomly(enemy, dungeon); 
            return;
        }
        if (enemy.LastHeardSound != null)
        {
            bool moved = MoveAwayFrom(enemy, dungeon, enemy.LastHeardSound.SourceX, enemy.LastHeardSound.SourceY);
            if (moved) Logger.Instance.Log($"[STRACH] {enemy.Name} ucieka od źródła dźwięku!");
            else enemy.LastHeardSound = null;
            return;
        }
        MoveRandomly(enemy, dungeon);
    }
}