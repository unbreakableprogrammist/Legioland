namespace Gra;
 
public class Goals : Items
{
    private int _amount; // ile go mamy 
    public Goals(int amount) // konstruktor
    {
        Name = $"Goals ({amount})";
        _amount = amount;
    }

    public override char GetSymbol() => 'G'; // Małe 'o'

    public override void PickUp(Player player) // implementujemy jak wyglada picup
    {
        player.Goals += _amount; 
    }
}

