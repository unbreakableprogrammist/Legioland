
namespace Gra;

public abstract class Weapon : Items 
{
    public bool IsTwoHanded { get; private set; } 
    public int MaxSlots { get; private set; } 
    private List<Items> _slottedItems = new List<Items>();
    private char _symbol; 
    private string _baseName; 
    public abstract int NoiseRange { get; } 
    public override bool CanBeSlotted => false; 

    public Weapon(string name, char symbol, int damage, bool isTwoHanded, int maxSlots = 0) 
    {
        _baseName = name; 
        _symbol = symbol;
        Damage = damage; 
        IsTwoHanded = isTwoHanded;
        MaxSlots = maxSlots;
    }
    public override int StrengthModifier => _slottedItems.Sum(i => i.StrengthModifier);
    public override int DexterityModifier => _slottedItems.Sum(i => i.DexterityModifier);
    public override int WisdomModifier => _slottedItems.Sum(i => i.WisdomModifier);
    public override int LuckModifier => _slottedItems.Sum(i => i.LuckModifier);
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
    public abstract string TypBroni { get; }
    public override string Name => _slottedItems.Any()
        ? $"{_baseName} ({TypBroni}) [Atk: {Damage}] <Zawiera: {string.Join(", ", _slottedItems.Select(i => i.Name))}>"
        : $"{_baseName} ({TypBroni}) [Atk: {Damage}]";
    public override char GetSymbol() => _symbol; 

    public abstract int AcceptAttack(IAttackVisitor visitor);
    public abstract int AcceptDefense(IDefenseVisitor visitor, Player player);
    
    public override void PickUp(Player player)
    {
        player.Backpack.Add(this); 
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
