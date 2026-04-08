namespace Gra.Decorator;

// --- 1. PECHOWY DEKORATOR ---
public class UnluckyDecorator : ItemDecorator
{
    public UnluckyDecorator(Items item) : base(item) { }

    public override string Name => base.Name + " (Unlucky)";

    public override void Equip(Player player, bool toRightHand)
    {
        base.Equip(player, toRightHand);
        player.Luck -= 5;
    } 
}