using Gra.Logging;
using Gra.Map;

namespace Gra.Movement;

public class DropCommand : ICommand
{
    private Player _player;
    private Dungeon _dungeon;
    
    public DropCommand(Player player, Dungeon dungeon)
    {
        _player = player;
        _dungeon = dungeon;
    }

    public void Execute()
    {
        if (_player.Backpack.Count == 0)
        {
            Logger.Instance.Log("Proba wyrzucenia przedmiotu, ale plecak jest pusty!");
            return;
        }
        Items itemDrop = _player.Backpack[_player.SelectedInventorySlot];
        _player.Backpack.RemoveAt(_player.SelectedInventorySlot);
        _dungeon.Grid[_player.X, _player.Y] = _dungeon.Grid[_player.X, _player.Y].ReceiveItem(itemDrop);
        _player.ClampInventorySelection();
        Logger.Instance.Log($"Wyrzucono przedmiot: {itemDrop.Name} na pozycje ({_player.X}, {_player.Y})");
    }
}