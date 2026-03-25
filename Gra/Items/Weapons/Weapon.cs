namespace Gra;

public class Weapon : Items 
{
    public int Damage { get; private set; } // nasz damage ( teraz nie uzywany )
    public bool IsTwoHanded { get; private set; } // dla itemkow dwu recznych
    private char _symbol; // symbol wyswietlania

    public Weapon(string name, char symbol, int damage, bool isTwoHanded) // konstruktor
    {
        Name = name;
        _symbol = symbol;
        Damage = damage;
        IsTwoHanded = isTwoHanded;
    }

    public override char GetSymbol() => _symbol; 

    public override void PickUp(Player player)
    {
        player.Backpack.Add(this); // Po podniesieniu trafia do plecaka
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