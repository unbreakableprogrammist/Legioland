
namespace Gra;

public abstract class Weapon : Items 
{
    public bool IsTwoHanded { get; private set; } 
    
    private char _symbol; 

    public Weapon(string name, char symbol, int damage, bool isTwoHanded) 
    {
        BaseName = name;
        _symbol = symbol;
        Damage = damage; 
        IsTwoHanded = isTwoHanded;
    }

    public override string Name => $"{BaseName} [Atk: {Damage}]";

    public override char GetSymbol() => _symbol; 

    public abstract int AcceptAttack(IAttackVisitor visitor);

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

public class HeavyWeapon : Weapon
{
    public HeavyWeapon(string name, char symbol, int damage, bool isTwoHanded) 
        : base(name, symbol, damage, isTwoHanded) { }

    public override int AcceptAttack(IAttackVisitor visitor) => visitor.Visit(this);
}

public class LightWeapon : Weapon
{
    public LightWeapon(string name, char symbol, int damage, bool isTwoHanded) 
        : base(name, symbol, damage, isTwoHanded) { }

    public override int AcceptAttack(IAttackVisitor visitor) => visitor.Visit(this);
}

public class MagicWeapon : Weapon
{
    public MagicWeapon(string name, char symbol, int damage, bool isTwoHanded) 
        : base(name, symbol, damage, isTwoHanded) { }

    public override int AcceptAttack(IAttackVisitor visitor) => visitor.Visit(this);
}