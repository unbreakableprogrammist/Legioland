namespace Gra;

public class HeavyWeapon : Weapon
{
    public HeavyWeapon(string name, char symbol, int damage, bool isTwoHanded) 
        : base(name, symbol, damage, isTwoHanded) { }

    public override int AcceptAttack(IAttackVisitor visitor) => visitor.Visit(this);
    public override int AcceptDefense(IDefenseVisitor visitor, Player player) => visitor.Visit(this, player);
}