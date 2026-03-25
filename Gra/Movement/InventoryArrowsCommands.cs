using Gra.Map;

namespace Gra.Movement
{
    // Przewijanie plecaka w górę
    public class InventoryUpCommand : ICommand
    {
        private Player _player;

        public InventoryUpCommand(Player player)
        {
            _player = player;
        }

        public void Execute()
        {
            if (_player.Backpack.Count > 0)
            {
                _player.SelectedInventorySlot--;
                if (_player.SelectedInventorySlot < 0) 
                    _player.SelectedInventorySlot = _player.Backpack.Count - 1;
            }
        }
    }

    // Przewijanie plecaka w dół
    public class InventoryDownCommand : ICommand
    {
        private Player _player;

        public InventoryDownCommand(Player player)
        {
            _player = player;
        }

        public void Execute()
        {
            if (_player.Backpack.Count > 0)
            {
                _player.SelectedInventorySlot++;
                
                if (_player.SelectedInventorySlot >= _player.Backpack.Count) 
                    _player.SelectedInventorySlot = 0;
            }
        }
    }
    public class GroundSelectLeftCommand : ICommand
    {
        private Player _player; 
        private Dungeon _dungeon;

        public GroundSelectLeftCommand(Player player, Dungeon dungeon)
        {
            _player = player;
            _dungeon = dungeon;
        }

        public void Execute()
        {
            // Sprawdzamy ile przedmiotów leży pod nogami
            int count = _dungeon.Grid[_player.X, _player.Y].GetItemNames().Count;
            if (count > 0)
            {
                _player.SelectedGroundSlot--;
                if (_player.SelectedGroundSlot < 0) _player.SelectedGroundSlot = count - 1; // Zawijanie
            }
        }
    }

    public class GroundSelectRightCommand : ICommand
    {
        private Player _player;
        private Dungeon _dungeon;

        public GroundSelectRightCommand(Player player, Dungeon dungeon)
        {
            _player = player;
            _dungeon = dungeon;
        }

        public void Execute()
        {
            int count = _dungeon.Grid[_player.X, _player.Y].GetItemNames().Count;
            if (count > 0)
            {
                _player.SelectedGroundSlot++;
                if (_player.SelectedGroundSlot >= count) _player.SelectedGroundSlot = 0; // Zawijanie
            }
        }
    }
}