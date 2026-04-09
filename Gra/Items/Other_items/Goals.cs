namespace Gra;
 
public class Goals : Items
{
    private int _amount; 
    public Goals(int amount) 
    {
        Name = $"Goals ({amount})";
        _amount = amount;
    }

    public override char GetSymbol() => 'G'; 

    public override void PickUp(Player player) 
    {
        player.Goals += _amount; 
    }
}

