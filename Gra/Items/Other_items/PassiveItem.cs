namespace Gra;

/// <summary>
/// klasa implementujaca kamienie 
/// </summary>
public class PassiveItem : Items
{
    private char _symbol;
    private int _str, _dex, _wis, _luk;

    public PassiveItem(string name, char symbol, int str = 0, int dex = 0, int wis = 0, int luk = 0)
    {
        Name = name;
        _symbol = symbol;
        _str = str; 
        _dex = dex; 
        _wis = wis; 
        _luk = luk;
    }

    public override int StrengthModifier => _str;
    public override int DexterityModifier => _dex;
    public override int WisdomModifier => _wis;
    public override int LuckModifier => _luk;

    public override char GetSymbol() => _symbol;

    public override void PickUp(Player player)
    {
        player.Backpack.Add(this);
    }
}