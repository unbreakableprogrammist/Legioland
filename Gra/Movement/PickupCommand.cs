using Gra.Logging;
using Gra.Map;
using Gra.Observer;

namespace Gra.Movement;

public class PickUpCommand : ICommand
{
    private Player _player;
    private Dungeon _dungeon;
    private readonly ISubject<SoundPayload> _soundNetwork; 

    public PickUpCommand(Player player, Dungeon dungeon, ISubject<SoundPayload> soundNetwork)
    {
        _player = player;
        _dungeon = dungeon;
        _soundNetwork = soundNetwork;
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

        if (podniesiony is Weapon weapon)
        {
            var payload = new SoundPayload(_player.X,_player.Y,weapon.NoiseRange);
            _soundNetwork.Notify(payload);
        }
    }
}
