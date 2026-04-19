namespace Gra;

public class ThemedEnemy : Enemy
{
    public ThemedEnemy(int x, int y, string name, char symbol, int health, int damage, int armor, IDefenseVisitor attackStyle) 
        : base(x, y)
    {
        Name = name;
        Symbol = symbol;
        Health = health;
        BaseDamage = damage;
        Armor = armor;
        AttackStyle = attackStyle;
    }
}