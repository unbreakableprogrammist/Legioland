namespace Gra;

public class WallCell : Cell
{
    public WallCell(int x, int y) : base(x, y) { } // X i Y jest dziedziczony 

    public override char GetSymbol() => '█';
    public override bool IsPassable() => false;
    public override Items TakeItem(int select_spot) => null;
    public override Cell ReceiveItem(Items item) => this; // zwracamy sama siebie bo i tak gracz tu nie wejdzie
}