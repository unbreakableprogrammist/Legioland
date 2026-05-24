namespace Gra.Movement;

public class ChangeStyleCommand : ICommand
{
    private Player _player;
    private IAttackVisitor _style;

    public ChangeStyleCommand(Player player, IAttackVisitor style)
    {
        _player = player;
        _style = style;
    }

    public void Execute()
    {
        _player.CurrentAttack = _style;
    }
}