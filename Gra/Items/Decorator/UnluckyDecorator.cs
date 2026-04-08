namespace Gra.Decorator;

// --- 1. PECHOWY DEKORATOR ---
public class UnluckyDecorator : ItemDecorator
{
    public UnluckyDecorator(Items item) : base(item) { }
    
    // Dokleja tekst do nazwy
    public override string Name => base.Name + " (Pechowy)";
    
    // Odejmuje 5 od modyfikatora szczęścia oryginalnej broni
    public override int LuckModifier => base.LuckModifier - 5;
}