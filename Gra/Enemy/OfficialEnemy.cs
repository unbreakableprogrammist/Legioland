using Gra.Behaviors;
using Gra.Logging;
using Gra.Map;
using Gra.Observer;

namespace Gra;

public class OfficialEnemy : Enemy
{
    public OfficialEnemy(int x, int y, string name, char symbol, int health, int damage, int armor, IDefenseVisitor attackStyle, ISubject<DeathPayload> deathSubject, ISubject<SoundPayload> soundSubject, Dungeon dungeon) 
        : base(x, y, deathSubject, soundSubject, dungeon, health)
    {
        Name = name;
        Symbol = symbol;
        Health = health;
        BaseDamage = damage;
        Armor = armor;
        AttackStyle = attackStyle;
        this.CurrentBehavior = new CowardlyBehavior();
        Health = health;
    }

    public override void OnNotify(DeathPayload message)
    {
        if (IsDead) return;

        BaseDamage = System.Math.Max(1, BaseDamage - 5); 

        Logger.Instance.Log($"[GATUNEK] Zginął Działacz! {Name} traci pewność siebie (obrażenia spadają do {BaseDamage}).");
    }
}