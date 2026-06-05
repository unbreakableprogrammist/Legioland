namespace Gra;

using Gra.Map;
using Gra.Observer;
using Gra.Behaviors;
using Gra.Logging;

public class NeutralEnemy : Enemy
{
    public NeutralEnemy(int x, int y, string name, char symbol, int health, int armor, int baseDamage, IDefenseVisitor attackStyle, ISubject<DeathPayload> deathSubject, ISubject<SoundPayload> soundSubject, Dungeon dungeon) 
        : base(x, y, deathSubject, soundSubject, dungeon, health) // Przekazujemy health do bazy!
    {
        Name = name;
        Symbol = symbol;
        Armor = armor;
        BaseDamage = baseDamage;
        AttackStyle = attackStyle;
        
        // Stan początkowy: Neutralny (ignoruje gracza, chodzi losowo)
        CurrentBehavior = new RandomBehavior();
    }

    public override void OnNotify(DeathPayload message) { }

    // --- MAGIA WZORCA STAN ---
    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage); // Najpierw odejmujemy mu życie
        CheckStateTransition();  // A potem sprawdzamy, czy zmienia zachowanie!
    }

    private void CheckStateTransition()
    {
        if (IsDead) return;

        if (Health >= MaxHealth / 2)
        {
            // Powyżej 50% lub równe = Wpada w Szał
            if (!(CurrentBehavior is AggressiveBehavior)) 
            {
                CurrentBehavior = new AggressiveBehavior();
                Logger.Instance.Log($"[STAN] {Name} wpadł w SZAŁ po ataku! Staje się agresywny!");
            }
        }
        else
        {
            // Poniżej 50% = Panikuje i ucieka
            if (!(CurrentBehavior is CowardlyBehavior))
            {
                CurrentBehavior = new CowardlyBehavior();
                Logger.Instance.Log($"[STAN] {Name} wpada w PANIKĘ! Zaczyna uciekać przed Tobą!");
            }
        }
    }
}