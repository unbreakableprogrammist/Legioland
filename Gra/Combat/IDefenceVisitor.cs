namespace Gra; 

public interface IDefenseVisitor
{
    
    int Visit(HeavyWeapon weapon, Player player);
    int Visit(LightWeapon weapon, Player player);
    int Visit(MagicWeapon weapon, Player player);
    int Visit(Items item, Player player); 
}