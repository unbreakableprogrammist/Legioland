using Gra.Logging;
using Gra.Map;

namespace Gra.Movement;

public class MoveCommand : ICommand
{
    private Player _player;
    private Dungeon _dungeon;
    private int _dx;
    private int _dy;
    
    public MoveCommand(Player player, Dungeon dungeon,int dx,int dy)
    {
        _player = player;
        _dungeon = dungeon;
        _dx = dx;
        _dy = dy;
    }

    public void Execute()
    {
        _player.Move(_dungeon, _dx, _dy);
        Logger.Instance.Log($"Gracz przesunal sie na pole ({_player.X},{_player.Y})");
    }
}