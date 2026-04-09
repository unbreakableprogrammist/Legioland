namespace Gra;

public class WallCell : Cell
{
    public WallCell(int x, int y) : base(x, y) { } 

    public override char GetSymbol() => '█';
    public override bool IsPassable() => false;
    public override Items TakeItem(int select_spot) => null;
    public override Cell ReceiveItem(Items item) => this; 
}