namespace Gra;

public class EmptyCell : Cell
{
    public EmptyCell(int x, int y) : base(x, y) { }
    
    public override char GetSymbol() => ' ';
    public override bool IsPassable() => true;
    public override Items TakeItem(int selected_spot) => null; // jesli jest pusta to zwracamy null
    public override Cell ReceiveItem(Items item)  // jesli przyjmujemy item 
    {
        ItemCell nowaKomorka = new ItemCell(X, Y); // tworzymy nowy obiekt ale o tym samym x i y
        nowaKomorka.AddItem(item); // dodajemy na nia item 
        // Zwracamy nową komórkę, żeby zastąpiła EmptyCell ItemCellem
        return nowaKomorka; 
    }
}