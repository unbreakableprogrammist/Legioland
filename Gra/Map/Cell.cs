namespace Gra;

public abstract class Cell 
{
    public int X { get; private set; } 
    public int Y { get; private set; }

    public Cell(int x, int y)
    {
        X = x;
        Y = y;
    }

    
    public abstract char GetSymbol(); 
    public abstract bool IsPassable(); 
    
    public virtual List<string> GetItemNames() 
    {
        return new List<string>(); 
    }
    public abstract Items TakeItem(int selected_spot); 
    public abstract Cell ReceiveItem(Items item); 
}