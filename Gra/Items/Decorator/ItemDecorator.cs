namespace Gra.Decorator;
public abstract class ItemDecorator : Items
{
    protected Items _wrapper;

    public ItemDecorator(Items item)
    {
        _wrapper = item;
    }

    
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
        
        
        _wrapper.Equip(player, toRightHand);
        
        
        if (player.LeftHand == _wrapper) player.LeftHand = this;
        if (player.RightHand == _wrapper) player.RightHand = this;
        
        
        if (wasInBackpack) player.Backpack.Remove(this);
    }
    
    public override int AcceptAttack(IAttackVisitor visitor)
    {
        
        
        return _wrapper.AcceptAttack(visitor); 
    }

    public override int AcceptDefense(IDefenseVisitor visitor, Player player)
    {
        return _wrapper.AcceptDefense(visitor, player);
    }
}