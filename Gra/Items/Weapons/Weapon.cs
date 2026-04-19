
namespace Gra;

public abstract class Weapon : Items 
{
    public bool IsTwoHanded { get; private set; } 
    
    private char _symbol; 
    private string _baseName; 

    public Weapon(string name, char symbol, int damage, bool isTwoHanded) 
    {
        _baseName = name; 
        _symbol = symbol;
        Damage = damage; 
        IsTwoHanded = isTwoHanded;
    }
    public abstract string TypBroni { get; }
    public override string Name => $"{_baseName} ({TypBroni}) [Atk: {Damage}]";
    public override char GetSymbol() => _symbol; 

    public abstract int AcceptAttack(IAttackVisitor visitor);
    public abstract int AcceptDefense(IDefenseVisitor visitor, Player player);
    
    public override void PickUp(Player player)
    {
        player.Backpack.Add(this); 
    }

    public override void Equip(Player player, bool toRightHand) 
    {
        if (IsTwoHanded)
        {
            if (player.LeftHand != null) player.Backpack.Add(player.LeftHand);
            if (player.RightHand != null && player.RightHand != player.LeftHand) 
                player.Backpack.Add(player.RightHand);
            player.LeftHand = this;
            player.RightHand = this;
        }
        else 
        {
            if (player.LeftHand != null && player.LeftHand == player.RightHand)
            {
                player.Backpack.Add(player.LeftHand);
                player.LeftHand = null;
                player.RightHand = null;
            }
            if (toRightHand)
            {
                if (player.RightHand != null) player.Backpack.Add(player.RightHand);
                player.RightHand = this;
            }
            else
            {
                if (player.LeftHand != null) player.Backpack.Add(player.LeftHand);
                player.LeftHand = this;
            }
        }
        player.Backpack.Remove(this);
    }
}
