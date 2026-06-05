using Gra.Behaviors;
using Gra.Logging;
using Gra.Map;
using Gra.Observer;

namespace Gra;


public abstract class Enemy : Gra.Observer.IObserver<SoundPayload>, Gra.Observer.IObserver<DeathPayload>{
    private protected ISubject<DeathPayload> _deathSubject;
    private protected ISubject<SoundPayload> _soundSubject ;
    private Dungeon _dungeon;
    public string Name { get; protected set; }
    public char Symbol { get; protected set; }
    public int Health { get; set; }
    public int Armor { get; set; }
    public int BaseDamage { get; protected set; }
    public int X { get; set; }
    public int Y { get; set; }
    public int MaxHealth { get; protected set; }
    
    public IEnemyBehavior CurrentBehavior { get; set; } = new RandomBehavior();
    public SoundPayload LastHeardSound { get; set; } = null; 
    
    public IDefenseVisitor AttackStyle { get; protected set; }

    public Enemy(int x, int y,ISubject<DeathPayload> deathSubject,ISubject<SoundPayload> soundSubject,Dungeon dungeon,int health)
    {
        X = x;
        Y = y;
        _deathSubject = deathSubject;
        _soundSubject = soundSubject;
        _dungeon = dungeon;
        _soundSubject.Attach(this);
        _deathSubject.Attach(this);
        
        Health = health; 
        MaxHealth = health; 
    }

    public virtual void TakeDamage(int damage)
    {
        Health -= damage;
    }

    public abstract void OnNotify(DeathPayload message);

    public virtual void OnNotify(SoundPayload message)
    {
        int distance = _dungeon.CalculatePathDistance(X, Y, message.SourceX, message.SourceY, message.Range);
        if (distance != -1 && distance <= message.Range)
        {
            LastHeardSound = message; 
            Logger.Instance.Log($"[DŹWIĘK] {Name} na pozycji ({X},{Y}) usłyszał hałas z ({message.SourceX},{message.SourceY}).");
        }
    }

    public bool IsDead => Health <= 0;

    public virtual void Die()
    {
        _deathSubject?.Notify(new DeathPayload());
        _deathSubject.Detach(this);
        _soundSubject.Detach(this);
    }

    public virtual void Move(Dictionary<int, Player> players)
    {
        if (IsDead) return;
        CurrentBehavior.ExecuteTurn(this, _dungeon, players);
    }
}
