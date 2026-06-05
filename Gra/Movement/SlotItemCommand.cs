using Gra.Logging;

namespace Gra.Movement;

public class SlotItemCommand : ICommand
{
    private Player _player;
    private bool _toLeftHand;

    public SlotItemCommand(Player player, bool toLeftHand)
    {
        _player = player;
        _toLeftHand = toLeftHand;
    }

    public void Execute()
    {
        if (_player.Backpack.Count == 0) return; 

        Items itemToSlot = _player.Backpack[_player.SelectedInventorySlot];
        Items targetWeapon = _toLeftHand ? _player.LeftHand : _player.RightHand;

        if (targetWeapon == null)
        {
            Logger.Instance.Log("Nie masz założonej broni w tej ręce, by włożyć do niej przedmiot!");
            return;
        }
        bool success = targetWeapon.AddToSlot(itemToSlot);

        if (success)
        {
            _player.Backpack.RemoveAt(_player.SelectedInventorySlot);
            _player.ClampInventorySelection();
            Logger.Instance.Log($"Pomyślnie osadzono: {itemToSlot.Name} w {targetWeapon.Name}!");
        }
        else
        {
            Logger.Instance.Log(
                $"Nie udało się! Przedmiot {targetWeapon.Name} nie posiada wolnych slotów lub tego przedmiotu nie można tam włożyć.");
        }
    }
}