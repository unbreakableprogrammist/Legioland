using System.Collections.Generic;
using System.Linq;

namespace Gra;

public class ItemCell : Cell
{
    
    private List<Items> _items = new List<Items>();
    public ItemCell(int x, int y) : base(x, y) { }

    public void AddItem(Items item) 
    {
        _items.Add(item);
    }

    
    public override Items TakeItem(int selected_spot)
    {
        if (_items.Count > 0) 
        {
            Items topItem = _items[selected_spot]; 
            _items.RemoveAt(selected_spot); 
            return topItem; 
        }
        return null; 
    }

    
    public override char GetSymbol()
    {
        if (_items.Count > 0) 
        {
            return _items.Last().GetSymbol(); 
        }
        return ' '; 
    }

    
    public override List<string> GetItemNames()
    {
        List<string> names = new List<string>();
        foreach (var item in _items)
        {
            names.Add(item.Name); 
        }
        return names;
    }

    public override bool IsPassable() => true;
    public override Cell ReceiveItem(Items item) 
    {
        AddItem(item); 
        return this;   
    }
}