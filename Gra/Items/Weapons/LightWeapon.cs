namespace Gra;

public class LightWeapon : Weapon
{
    public override string TypBroni => "Lekka";
    public override int NoiseRange => 7;

    public LightWeapon(string name, char symbol, int damage, bool isTwoHanded) 
        : base(name, symbol, damage, isTwoHanded) { }

    public override int AcceptAttack(IAttackVisitor visitor) => visitor.Visit(this);
    public override int AcceptDefense(IDefenseVisitor visitor, Player player) => visitor.Visit(this, player);
}