namespace Gra;
 
// na razie zeby nie zasmiecac programu 100 plikow gold i money beda tutaj jak sie rozrosnie ich uzywalnosc to rozdziele to na klasy 
public class Coin : Items
{
    private int _amount; // ile go mamy 
    public Coin(int amount) // konstruktor
    {
        Name = $"Monety ({amount})";
        _amount = amount;
    }

    public override char GetSymbol() => 'o'; // Małe 'o'

    public override void PickUp(Player player) // implementujemy jak wyglada picup
    {
        player.Coins += _amount; 
    }
}

public class Gold : Items
{
    private int _amount;

    public Gold(int amount)
    {
        Name = $"Zloto ({amount})";
        _amount = amount;
    }

    public override char GetSymbol() => '*'; 

    public override void PickUp(Player player)
    {
        player.Gold += _amount; 
    }
}