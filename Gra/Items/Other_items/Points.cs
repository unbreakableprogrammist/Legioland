namespace Gra;

public class Points : Items
{
    private int _amount;

    public Points(int amount)
    {
        Name = $"Points ({amount})";
        _amount = amount;
    }

    public override char GetSymbol() => 'P';

    public override void PickUp(Player player)
    {
        player.Points += _amount;
    }
}