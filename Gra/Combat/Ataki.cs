namespace Gra; 


public class AtakZwyklyVisitor : IAttackVisitor
{
    public int Visit(HeavyWeapon weapon) => weapon.Damage;
    public int Visit(LightWeapon weapon) => weapon.Damage;
    public int Visit(MagicWeapon weapon) => 1;
    public int Visit(Items item) => item.Damage; 
}


public class AtakSkrytyVisitor : IAttackVisitor
{
    public int Visit(HeavyWeapon weapon) => weapon.Damage / 2; 
    public int Visit(LightWeapon weapon) => weapon.Damage * 2; 
    public int Visit(MagicWeapon weapon) => 1;
    public int Visit(Items item) => item.Damage;
}


public class AtakMagicznyVisitor : IAttackVisitor
{
    public int Visit(HeavyWeapon weapon) => 1;
    public int Visit(LightWeapon weapon) => 1;
    public int Visit(MagicWeapon weapon) => weapon.Damage;     
    public int Visit(Items item) => item.Damage;
}