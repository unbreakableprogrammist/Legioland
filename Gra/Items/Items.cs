namespace Gra;

public abstract class Items 
{
    public virtual string Name { get; protected set; } 
    
    public virtual int Damage { get; protected set; } = 0;
    public virtual bool IsTwoHanded => false;
    public virtual int LuckModifier => 0; 
    
    public virtual int StrengthModifier => 0;
    public virtual int DexterityModifier => 0;
    public virtual int WisdomModifier => 0;
    public virtual bool CanBeSlotted => true; // czy bron moze miec sloty 
    public virtual bool AddToSlot(Items item) => false; // zwraca false jak nie ma slotow
    public virtual List<Items> GetSlottedItems() => new List<Items>();    
    public abstract char GetSymbol(); 
    public abstract void PickUp(Player player); 
    public virtual void Equip(Player player, bool toRightHand) { }
    public virtual int AcceptAttack(IAttackVisitor visitor) => visitor.Visit(this);
    public virtual int AcceptDefense(IDefenseVisitor visitor, Player player) => visitor.Visit(this, player);
}