using Gra.Behaviors;
using Gra.Logging;
using Gra.Map;
using Gra.Observer;

namespace Gra;

public class ThemedEnemy : Enemy
{
    public ThemedEnemy(int x, int y, string name, char symbol, int health, int damage, int armor, IDefenseVisitor attackStyle,ISubject<DeathPayload> deathSubject,ISubject<SoundPayload> soundSubject,Dungeon dungeon) 
        : base(x, y,deathSubject,soundSubject,dungeon,health)
    {
        Name = name;
        Symbol = symbol;
        Health = health;
        BaseDamage = damage;
        Armor = armor;
        AttackStyle = attackStyle;
        this.CurrentBehavior = new AggressiveBehavior();
    }

    public override void OnNotify(DeathPayload message)
    {
        if (IsDead) return;
        BaseDamage += 2; 
        Logger.Instance.Log($"[GATUNEK] Zginął inny Klub! {Name} wpada w szał bojowy! Jego obrażenia rosną do {BaseDamage}.");
    }
}