using Gra.Logging;
using Gra.Map;
using Gra.Observer;

namespace Gra;


public abstract class Enemy : Gra.Observer.IObserver<SoundPayload>, Gra.Observer.IObserver<DeathPayload>{
    // subskrybujemy oba kanaly 
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

    
    public IDefenseVisitor AttackStyle { get; protected set; }

    public Enemy(int x, int y,ISubject<DeathPayload> deathSubject,ISubject<SoundPayload> soundSubject,Dungeon dungeon)
    {
        X = x;
        Y = y;
        _deathSubject = deathSubject;
        _soundSubject = soundSubject;
        _dungeon = dungeon;
        _soundSubject.Attach(this);
        _deathSubject.Attach(this);
    }

    public abstract void OnNotify(DeathPayload message);

    public virtual void OnNotify(SoundPayload message)
    {
        int distance = _dungeon.CalculatePathDistance(X,Y,message.SourceX, message.SourceY,message.Range);
        if (distance != -1 && distance <= message.Range)
        {
            Logger.Instance.Log($"[DŹWIĘK] {Name} na pozycji ({X},{Y}) usłyszał hałas z ({message.SourceX},{message.SourceY}). Odległość: {distance}.");
        }
    }

    public bool IsDead => Health <= 0;

    public virtual void Die()
    {
        _deathSubject?.Notify(new DeathPayload());
        _deathSubject.Detach(this);
        _soundSubject.Detach(this);
    }

    public virtual void Move()
    {
        if(IsDead) return;
        
        int[] dx = { 0, 0, 1, -1};
        int[] dy = { 1, -1, 0, 0};

        Random rnd = new Random();
        int direction = rnd.Next(4);
        
        int newX = X + dx[direction];
        int newY = Y + dy[direction];

        if (newX >= 0 && newX < _dungeon.Width && newY >= 0 && newY < _dungeon.Height &&
            _dungeon.Grid[newX, newY].IsPassable())
        {
            X = newX;
            Y = newY;
            Logger.Instance.Log($"Potwor przesunal sie na pole {X},{Y}");
        }        
    }
}
