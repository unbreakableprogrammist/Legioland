using Gra.Logging;
using Gra.Map;

namespace Gra.Movement;

public class PickUpCommand : ICommand
{
    private Player _player;
    private Dungeon _dungeon;

    public PickUpCommand(Player player, Dungeon dungeon)
    {
        _player = player;
        _dungeon = dungeon;
    }

    public void Execute()
    {
        var cell = _dungeon.Grid[_player.X, _player.Y];
        Items podniesiony = cell.TakeItem(_player.SelectedGroundSlot);
        if (podniesiony != null)
        {
            podniesiony.PickUp(_player); 
            _player.ClampGroundSelection(cell.GetItemNames().Count); 
            Logger.Instance.Log($"Podniesiono przedmiot: {podniesiony.Name}");
        }
        else 
        {
            Logger.Instance.Log("Gracz probowal cos podniesc, ale nic tam nie bylo.");
        }
    }
}