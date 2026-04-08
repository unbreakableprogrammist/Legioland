namespace Gra.Decorator;
public abstract class ItemDecorator : Items
{
    protected Items _wrapper;

    public ItemDecorator(Items item)
    {
        _wrapper = item;
    }

    // Przekazujemy wszystko w dół
    public override string Name => _wrapper.Name;
    public override int Damage => _wrapper.Damage;
    public override bool IsTwoHanded => _wrapper.IsTwoHanded;
    public override int LuckModifier => _wrapper.LuckModifier;
    public override char GetSymbol() => _wrapper.GetSymbol();

    public override void PickUp(Player player)
    {
        player.Backpack.Add(this);
    }

    public override void Equip(Player player, bool toRightHand)
    {
        bool wasInBackpack = player.Backpack.Contains(this);
        
        // Niech bazowa broń zrobi swoje (zdejmie inne bronie itp.)
        _wrapper.Equip(player, toRightHand);
        
        if (player.LeftHand == _wrapper) player.LeftHand = this;
        if (player.RightHand == _wrapper) player.RightHand = this;
        
        if (wasInBackpack) player.Backpack.Remove(this);
    }
}