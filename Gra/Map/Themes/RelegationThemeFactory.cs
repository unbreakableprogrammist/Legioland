namespace Gra.Map.Themes;

public class RelegationThemeFactory : IThemeFactory
{
    public string GetIntroMessage() => 
        "Choć czas trudny i czas zły to Legia walczy do końca!";

    public Items CreateArtefact() => 
        new MagicWeapon("Silna Obrona", 'O', 300, true);

    public Enemy CreateEnemy(int x, int y, Random rnd)
    {
        int los = rnd.Next(3);
        if (los == 0) return new ThemedEnemy(x, y, "Termalica Bruk-Bet", 't', 50,5, 15, new ObronaPrzedZwyklymVisitor());
        if (los == 1) return new ThemedEnemy(x, y, "Arka Gdynia", 'A', 140, 35, 15, new ObronaPrzedSkrytymVisitor());
        return new ThemedEnemy(x, y, "widzew ", '✡', 160, 20, 20, new ObronaPrzedZwyklymVisitor());
    }

    public Weapon CreateRandomWeapon(Random rnd)
    {
        int los = rnd.Next(3);
        if (los == 0) return new HeavyWeapon("Radovan Pankov", 'V', 50, true);   
        if (los == 1) return new HeavyWeapon("Kamil Piatkowski", 'P', 60, true); 
        return new MagicWeapon("Patryk Kun", 'K', 70, false);                  
    }
}