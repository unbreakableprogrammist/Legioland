using Gra.Logging;

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
        if (_player.Backpack.Count > 0) 
        {
            Items item = _player.Backpack[_player.SelectedInventorySlot];
            item.Equip(_player, _rightHand);
            _player.ClampInventorySelection();
            Logger.Instance.Log($"Wyposazono: {item.Name} w reke {(_rightHand ? "Prawa" : "Lewa")}");
        }
        else 
        {
            Logger.Instance.Log("Proba wyposazenia przedmiotu, ale plecak jest pusty!");
        }
    }
}