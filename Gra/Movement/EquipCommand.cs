namespace Gra.Movement;

public class EquipCommand : ICommand
{
    private Player _player;
    private bool _rightHand;

    public EquipCommand(Player player,bool rightHand)
    {
        _player = player;
        _rightHand = rightHand;
    }

    public void Execute()
    { 
        if (_player.Backpack.Count > 0) // jesli cos mamy w plecaku 
        {
            Items item = _player.Backpack[_player.SelectedInventorySlot];
            item.Equip(_player, _rightHand);
            _player.ClampInventorySelection();
        }
    }
}