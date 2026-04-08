using Gra.Map;

namespace Gra.Movement;

public class AttackCommand : ICommand
{
    private Player _player;
    private Dungeon _dungeon;
    private bool _useRightHand;
    private Action<string> _onMessage;

    public AttackCommand(Player player, Dungeon dungeon, bool useRightHand, Action<string> onMessage)
    {
        _player = player;
        _dungeon = dungeon;
        _useRightHand = useRightHand;
        _onMessage = onMessage;
    }

    public void Execute()
    {
        Enemy target = _dungeon.GetEnemyAt(_player.X, _player.Y);
        if (target == null) return;

        // Logika Visitora (Atak)
        Items weapon = _useRightHand ? _player.RightHand : _player.LeftHand;
        int damage = (weapon != null) ? weapon.AcceptAttack(_player.CurrentAttack) : _player.Strength / 2;
        
        target.Health -= damage;
        string report = $"Zadałeś {damage} obr. ";

        if (target.IsDead) 
        {
            report += "Wróg pokonany!";
            // Dungeon.Enemies.Remove(target) powinna być w logice walki lub tutaj
        }
        else 
        {
            // Kontratak (Visitor Obrony)
            int def = (_player.LeftHand != null) ? _player.LeftHand.AcceptDefense(target.AttackStyle, _player) : _player.Dexterity;
            int taken = Math.Max(0, target.BaseDamage - def);
            _player.Health -= taken;
            report += $"| Otrzymałeś {taken} obr.";
        }

        _onMessage(report);
    }
}