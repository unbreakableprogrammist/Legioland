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

    public override void Equip(Player player)
    {
        if (IsTwoHanded)
        {
            // Zdejmujemy wszystko z rąk z powrotem do plecaka
            if (player.LeftHand != null) player.Backpack.Add(player.LeftHand);
            // Upewniamy się, że nie dodamy dwa razy tej samej broni dwuręcznej do plecaka:
            if (player.RightHand != null && player.RightHand != player.LeftHand) player.Backpack.Add(player.RightHand);

            // Zajmujemy obie ręce TĄ SAMĄ bronią
            player.LeftHand = this;
            player.RightHand = this;
        }
        else 
        {
            // Próbujemy dać do lewej ręki
            if (player.LeftHand == null) 
            {
                player.LeftHand = this;
            }
            // Jak lewa zajęta, próbujemy do prawej
            else if (player.RightHand == null) 
            {
                player.RightHand = this;
            }
            // Jak obie zajęte, wyrzucamy to co w lewej do plecaka i bierzemy nową do lewej
            else
            {
                // Jeśli w rękach była broń dwuręczna, zdejmujemy z obu:
                if (player.LeftHand == player.RightHand)
                {
                    player.Backpack.Add(player.LeftHand);
                    player.LeftHand = null;
                    player.RightHand = null;
                }
                else
                {
                    player.Backpack.Add(player.LeftHand); // Ściągamy tylko z lewej
                }
                
                player.LeftHand = this; // Zakładamy nową
            }
        }
        
        // Na sam koniec: usuwamy tę broń z plecaka (bo jest już w rękach!)
        player.Backpack.Remove(this);
    }
}