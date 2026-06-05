using Gra.Logging;
using Gra.Map;

namespace Gra.Behaviors;

public class AggressiveBehavior : BaseEnemyBehavior
{
    public override void ExecuteTurn(Enemy enemy, Dungeon dungeon, Dictionary<int, Player> players)
    {
        if (enemy.IsDead) return;

        Player targetPlayer = GetClosestVisiblePlayer(enemy, dungeon, players);

        if (targetPlayer != null)
        {
            enemy.LastHeardSound = null; 
            int dist = System.Math.Abs(enemy.X - targetPlayer.X) + System.Math.Abs(enemy.Y - targetPlayer.Y);
            
            if (dist > 1) 
            {
                MoveTowards(enemy, dungeon, targetPlayer.X, targetPlayer.Y);
                Logger.Instance.Log($"[AGRESJA] {enemy.Name} biegnie w stronę gracza!");
            }
            return; 
        }

        // 2. Jeśli nie widzi, sprawdzamy czy coś SŁYSZAŁ
        if (enemy.LastHeardSound != null)
        {
            if (enemy.X == enemy.LastHeardSound.SourceX && enemy.Y == enemy.LastHeardSound.SourceY)
            {
                enemy.LastHeardSound = null; // Doszedł na miejsce dźwięku, nic nie ma, zapomina
            }
            else
            {
                MoveTowards(enemy, dungeon, enemy.LastHeardSound.SourceX, enemy.LastHeardSound.SourceY);
                Logger.Instance.Log($"[AGRESJA] {enemy.Name} podąża za dźwiękiem.");
                return; // Kończymy turę
            }
        }

        // 3. Jak nie ma gracza ani dźwięku - porusza się losowo (domyślne)
        MoveRandomly(enemy, dungeon);
    }
}