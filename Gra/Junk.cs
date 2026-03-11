namespace Gra;

// nieuzywane przedmioty po prostu maja imie symbol, na razie nie oplaca sie pisac oddzielnych klas do nich 
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