namespace Gra;

public abstract class Enemy
{
    public string Name { get; protected set; }
    public char Symbol { get; protected set; }
    public int Health { get; set; }
    public int BaseDamage { get; protected set; }
    public int X { get; set; }
    public int Y { get; set; }

    
    public IDefenseVisitor AttackStyle { get; protected set; }

    public Enemy(int x, int y)
    {
        X = x;
        Y = y;
    }

    public bool IsDead => Health <= 0;
}

public class ZlyPudel : Enemy
{
    public ZlyPudel(int x, int y) : base(x, y)
    {
        Name = "Zły Pudel";
        Symbol = 'U';
        Health = 150;
        BaseDamage = 30;
        AttackStyle = new ObronaPrzedZwyklymVisitor(); 
    }
}

public class Sedzia : Enemy
{
    public Sedzia(int x, int y) : base(x, y)
    {
        Name = "Sędzia Kalosz";
        Symbol = 'S';
        Health = 80;
        BaseDamage = 45;
        AttackStyle = new ObronaPrzedSkrytymVisitor(); 
    }
}