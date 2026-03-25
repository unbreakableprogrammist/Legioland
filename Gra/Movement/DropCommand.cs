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
        var cell = _dungeon.Grid[_player.X, _player.Y];
        
        if(_player.Backpack.Count == 0) return;
        
        Items itemDrop = _player.Backpack[_player.SelectedGroundSlot];
        _player.Backpack.RemoveAt(_player.SelectedGroundSlot);
        _dungeon.Grid[_player.X, _player.Y] = _dungeon.Grid[_player.X, _player.Y].ReceiveItem(itemDrop);
        _player.ClampInventorySelection();
    }
}