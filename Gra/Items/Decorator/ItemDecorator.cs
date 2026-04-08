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
        // IGNORUJEMY bazowe PickUp. Sami dodajemy NASZ DEKORATOR do plecaka!
        player.Backpack.Add(this);
    }

    public override void Equip(Player player, bool toRightHand)
    {
        bool wasInBackpack = player.Backpack.Contains(this);
        
        // Niech bazowa broń zrobi swoje (zdejmie inne bronie itp.)
        _wrapper.Equip(player, toRightHand);
        
        // Jeśli bazowa broń wskoczyła w ręce, zamieniamy ją na nasz DEKORATOR!
        if (player.LeftHand == _wrapper) player.LeftHand = this;
        if (player.RightHand == _wrapper) player.RightHand = this;
        
        // Jeśli byliśmy w plecaku, to się z niego usuwamy (bo bazowa broń usunęła tylko samą siebie)
        if (wasInBackpack) player.Backpack.Remove(this);
    }
}