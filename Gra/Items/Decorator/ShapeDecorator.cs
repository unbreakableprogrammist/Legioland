namespace Gra.Decorator;

public class ShapeDecorator : ItemDecorator
{
    public ShapeDecorator(Items item) : base(item) { }
    
    public override string Name => base.Name + " (Silny)";
    
    public override int Damage => base.Damage + 5;
}