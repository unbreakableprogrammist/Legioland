namespace Gra;

public class MagicWeapon : Weapon
{
    public override string TypBroni => "Magiczna";
    public override int NoiseRange => 5;

    public MagicWeapon(string name, char symbol, int damage, bool isTwoHanded,int maxSlots = 0) 
        : base(name, symbol, damage, isTwoHanded,maxSlots) { }

    public override int AcceptAttack(IAttackVisitor visitor) => visitor.Visit(this);
    public override int AcceptDefense(IDefenseVisitor visitor, Player player) => visitor.Visit(this, player);
}