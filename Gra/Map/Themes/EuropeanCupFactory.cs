namespace Gra.Map.Themes;

public class EuropeanCupFactory : IThemeFactory
{
    public string GetIntroMessage() => 
        "Wtorki i środy na sportowo! Jedyna drużyna godnie reprezentująca Polskę w Europie wkracza na salony";

    public Items CreateArtefact() => new HeavyWeapon("Ultrasi", 'U', 250, true);
    
    public Enemy CreateEnemy(int x,int y,Random rnd)
    {
        int los = rnd.Next(3);
        if (los == 0) return new ThemedEnemy(x, y, "Chelsea FC", 'C', 200, 60,60, new ObronaPrzedSkrytymVisitor());
        if (los == 1) return new ThemedEnemy(x, y, "Real Betis", 'B', 180, 50, 15,new ObronaPrzedMagicznymVisitor());
        
        return new ThemedEnemy(x, y, "Aston Villa", 'V', 190, 55, 30,new ObronaPrzedZwyklymVisitor());
    }
    public Weapon CreateRandomWeapon(Random rnd)
    {
        int los = rnd.Next(3);
        if (los == 0) return new MagicWeapon("Juergen Elitim", 'E', 70, false);     
        if (los == 1) return new HeavyWeapon("Jean-Pierre Nsame", 'N', 80, true);   
        return new LightWeapon("Vahan Bichakhchyan", 'V', 65, false);               
    }
    
}