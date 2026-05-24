using Gra.Map;

namespace Gra.Movement;

public class ToggleCombatCommand : ICommand
{
    private Player _player;
    private Dungeon _dungeon;

    public ToggleCombatCommand(Player player, Dungeon dungeon)
    {
        _player = player;
        _dungeon = dungeon;
    }

    public void Execute()
    {
        if (!_player.IsInCombatMode)
        {
            Enemy target = _dungeon.GetEnemyAt(_player.X, _player.Y);
            if (target != null) _player.IsInCombatMode = true;
        }
        else
        {
            _player.IsInCombatMode = false;
        }
    }
}