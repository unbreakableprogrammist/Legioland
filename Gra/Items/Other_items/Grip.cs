namespace Gra;

public class Grip : Items
{
    private char _symbol;
    private string _baseName;
    public int MaxSlots { get; private set; }
    private List<Items> _slottedItems = new List<Items>();

    public Grip(string name, char symbol, int maxSlots)
    {
        _baseName = name;
        _symbol = symbol;
        MaxSlots = maxSlots;
    }
    public override string Name => _slottedItems.Any()
        ? $"{_baseName} <Zawiera: {string.Join(", ", _slottedItems.Select(i => i.Name))}>"
        : _baseName;

    public override int StrengthModifier => _slottedItems.Sum(i => i.StrengthModifier);
    public override int DexterityModifier => _slottedItems.Sum(i => i.DexterityModifier);
    public override int WisdomModifier => _slottedItems.Sum(i => i.WisdomModifier);
    public override int LuckModifier => _slottedItems.Sum(i => i.LuckModifier);

    public override char GetSymbol() => _symbol;

    public override void PickUp(Player player)
    {
        player.Backpack.Add(this);
    }

    public override bool AddToSlot(Items item)
    {
        if (item.CanBeSlotted && _slottedItems.Count < MaxSlots)
        {
            _slottedItems.Add(item);
            return true;
        }
        return false;
    }

    public override List<Items> GetSlottedItems() => _slottedItems;
}
