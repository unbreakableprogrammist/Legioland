namespace Gra.Decorator;

public class ShapeDecorator : ItemDecorator
{
    public ShapeDecorator(Items item) : base(item) { }
    public override int Damage => base.Damage + 5;

    public override string Name => base.Name + " (Shape)";
    
}