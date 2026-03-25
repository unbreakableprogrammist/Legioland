namespace Gra;

public abstract class Cell // klasa po ktorej beda dziedziczyc przedmioty
{
    public int X { get; private set; } // jej x 
    public int Y { get; private set; }

    public Cell(int x, int y)
    {
        X = x;
        Y = y;
    }

    // Każde pole musi umieć zwrócić swój znak i powiedzieć, czy można na nie wejść
    public abstract char GetSymbol(); // zwraca swoj symbol 
    public abstract bool IsPassable(); // mowi czy mozna na nia wejsc
    
    public virtual List<string> GetItemNames() // funkcja ktora listuje itemy na komorce
    {
        return new List<string>(); // Domyślnie zwraca pustą listę
    }
    public abstract Items TakeItem(int selected_spot); // funkcja ktora implementuje jesli gracz nacisnie E
    public abstract Cell ReceiveItem(Items item); // funkcja ktora implementuje opuszczanie na komorke itemu
}