namespace Gra;

public class EmptyCell : Cell
{
    public EmptyCell(int x, int y) : base(x, y) { }
    
    public override char GetSymbol() => ' ';
    public override bool IsPassable() => true;
    public override Items TakeItem(int selected_spot) => null; 
    public override Cell ReceiveItem(Items item)  
    {
        ItemCell nowaKomorka = new ItemCell(X, Y); 
        nowaKomorka.AddItem(item); 
        
        return nowaKomorka; 
    }
}