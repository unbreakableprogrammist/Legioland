namespace Gra;

public abstract class Items // abstrakcyjna klasa na wszystkie itemy 
{
    public string Name { get; protected set; } // imie 
    public abstract char GetSymbol(); // symbol
    public abstract void PickUp(Player player); // jak dziala podnoszenie ( np cos do ekwipunku, inaczej dla money/gold)

    public virtual void Equip(Player player) // jak dziala equipowanie
    {
    }
}