using System.Collections.Generic;
using System.Linq;

namespace Gra;

public class ItemCell : Cell
{
    // lista itemow na naszej komorce
    private List<Items> _items = new List<Items>();
    public ItemCell(int x, int y) : base(x, y) { }

    public void AddItem(Items item) // dodajemy item do listy
    {
        _items.Add(item);
    }

    // implementacja podnoszenia przez gracza
    public override Items TakeItem()
    {
        if (_items.Count > 0) // jesli cos na nas jest
        {
            Items topItem = _items.Last(); // Bierzemy ostatni dodany element 
            _items.RemoveAt(_items.Count - 1); // Usuwamy z listy na ziemi
            return topItem; // Zwracamy do _gracz.PickUp()
        }
        return null; // Jak pusto, zwracamy null
    }

    // zwracanie symbolu itemow 
    public override char GetSymbol()
    {
        if (_items.Count > 0) // jesli sa tu jakies przedmioty
        {
            return _items.Last().GetSymbol(); // Rysuje znak przedmiotu na górze
        }
        return ' '; // Puste pole
    }

    // zwracanie nazw przedmiotow
    public override List<string> GetItemNames()
    {
        List<string> names = new List<string>();
        foreach (var item in _items)
        {
            names.Add(item.Name); // Zbieramy same nazwy dla naszego interfejsu
        }
        return names;
    }

    public override bool IsPassable() => true;
    public override Cell ReceiveItem(Items item) // jesli jestesmy item cellem to po prostu przyjmujemy przedmiot 
    {
        AddItem(item); // Dodajemy do naszej listy
        return this;   // Zwracamy samych siebie, nic się nie zmienia
    }
}