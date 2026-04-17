namespace Gra; 

public interface IAttackVisitor
{
    int Visit(HeavyWeapon weapon);
    int Visit(LightWeapon weapon);
    int Visit(MagicWeapon weapon);
    int Visit(Items item); 
}