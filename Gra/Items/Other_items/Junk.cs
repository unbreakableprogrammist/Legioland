namespace Gra;


public class Junk : Items
{
    public Junk(string name, char symbol)
    {
        Name = name;
        _symbol = symbol;
    }

    private char _symbol;
    public override char GetSymbol() => _symbol;

    public override void PickUp(Player player)
    {
        player.Backpack.Add(this); 
    }
}