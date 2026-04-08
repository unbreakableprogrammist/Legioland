namespace Gra;

public abstract class Items 
{
    public virtual string Name { get; protected set; } 
    
    public virtual int Damage { get; protected set; } = 0;
    public virtual bool IsTwoHanded => false;
    public virtual int LuckModifier => 0; 

    public abstract char GetSymbol(); 
    public abstract void PickUp(Player player); 
    public virtual void Equip(Player player, bool toRightHand) { }
}