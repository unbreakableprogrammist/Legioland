using Gra.Map;
namespace Gra;

public class Player
{
    public int X, Y;
    public int Strength { get; set; } = 10; 
    public int Dexterity { get; set; } = 10; 
    public int Health { get; set; } = 100; 
    public int Luck { get; set; } = 100; 
    public int Aggression { get; set; } = 5; 
    public int Wisdom { get; set; } = 5; 

    public int Points { get; set; } = 0; 
    public int Goals { get; set; } = 0;  

    public List<Items> Backpack { get; private set; } = new List<Items>();

    public Items LeftHand { get; set; } = null;
    public Items RightHand { get; set; } = null;
    
    
    public IAttackVisitor CurrentAttack { get; set; } = new AtakZwyklyVisitor();
    
    
    public bool IsInCombatMode { get; set; } = false;
    
    public int SelectedInventorySlot { get; set; } = 0;
    public int SelectedGroundSlot { get; set; } = 0;
    
    public Player(int x_get, int y_get)
    {
        X = x_get;
        Y = y_get;
    }

    public void Move(Dungeon dungeon,int dx, int dy)
    {
        if (dungeon.CanEnter(X+dx,Y+dy))
        {
            X+=dx;
            Y+=dy;
        }
    }
    public void ClampInventorySelection()
    {
        if (Backpack.Count == 0) SelectedInventorySlot = 0;
        else if (SelectedInventorySlot >= Backpack.Count) SelectedInventorySlot = Backpack.Count - 1;
        else if (SelectedInventorySlot < 0) SelectedInventorySlot = 0;
    }

    public void ClampGroundSelection(int itemsOnGroundCount)
    {
        if (itemsOnGroundCount == 0) SelectedGroundSlot = 0;
        else if (SelectedGroundSlot >= itemsOnGroundCount) SelectedGroundSlot = itemsOnGroundCount - 1;
        else if (SelectedGroundSlot < 0) SelectedGroundSlot = 0;
    }
}