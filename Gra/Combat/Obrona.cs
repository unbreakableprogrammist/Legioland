namespace Gra;


public class ObronaPrzedZwyklymVisitor : IDefenseVisitor
{
    public int Visit(HeavyWeapon weapon, Player player) => player.Strength + player.Luck;
    public int Visit(LightWeapon weapon, Player player) => player.Dexterity + player.Luck;
    public int Visit(MagicWeapon weapon, Player player) => player.Dexterity + player.Luck;
    public int Visit(Items item, Player player) => player.Dexterity; 
}


public class ObronaPrzedSkrytymVisitor : IDefenseVisitor
{
    public int Visit(HeavyWeapon weapon, Player player) => player.Strength;
    public int Visit(LightWeapon weapon, Player player) => player.Dexterity * 2;
    public int Visit(MagicWeapon weapon, Player player) => 0; 
    public int Visit(Items item, Player player) => 0;
}


public class ObronaPrzedMagicznymVisitor : IDefenseVisitor
{
    public int Visit(HeavyWeapon weapon, Player player) => player.Luck;
    public int Visit(LightWeapon weapon, Player player) => player.Luck;
    public int Visit(MagicWeapon weapon, Player player) => player.Wisdom * 2; 
    public int Visit(Items item, Player player) => player.Luck;
}