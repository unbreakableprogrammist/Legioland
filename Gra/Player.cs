namespace Gra;

public class Player
{
    public int X, Y;
    public int Strength { get; set; } = 10; // sila 
    public int Dexterity { get; set; } = 10; // zrecznosc
    public int Health { get; set; } = 100; // zdrowie 
    public int Luck { get; set; } = 5; // szczescie
    public int Aggression { get; set; } = 5; // agresja 
    public int Wisdom { get; set; } = 5; // madrosc 

    public int Coins { get; set; } = 100; // Monety
    public int Gold { get; set; } = 100;  // Złoto

    // Plecak na przedmioty (np. nieużywalne i bronie, których akurat nie trzymamy)
    public List<Items> Backpack { get; private set; } = new List<Items>();

    // Ręce gracza
    public Items LeftHand { get; set; } = null;
    public Items RightHand { get; set; } = null;
    public Player(int x_get, int y_get)
    {
        X = x_get;
        Y = y_get;
    }

    public void Move(int dx, int dy, Board board)
    {
        if (board.CanEnter(X+dx,Y+dy))
        {
            X+=dx;
            Y+=dy;
        }
        
        
    }

    
}