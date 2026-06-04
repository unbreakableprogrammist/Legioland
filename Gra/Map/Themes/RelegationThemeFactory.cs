using System;
using Gra.Map;
using Gra.Observer;


namespace Gra.Map.Themes;

public class RelegationThemeFactory : IThemeFactory
{
    public string GetIntroMessage() => 
        "Choć czas trudny i czas zły to Legia walczy do końca!";

    public Items CreateArtefact() => 
        new MagicWeapon("Silna Obrona", 'O', 300, true);

    public Enemy CreateClubEnemy(int x, int y, Random rnd, ISubject<DeathPayload> deathNetwork, ISubject<SoundPayload> soundNetwork, Dungeon dungeon)
    {
        int los = rnd.Next(3);
        if (los == 0) return new ThemedEnemy(x, y, "Termalica Bruk-Bet", 't', 50, 5, 15, new ObronaPrzedZwyklymVisitor(), deathNetwork, soundNetwork, dungeon);
        if (los == 1) return new ThemedEnemy(x, y, "Arka Gdynia", 'A', 140, 35, 15, new ObronaPrzedSkrytymVisitor(), deathNetwork, soundNetwork, dungeon);
        return new ThemedEnemy(x, y, "widzew ", '✡', 160, 20, 20, new ObronaPrzedZwyklymVisitor(), deathNetwork, soundNetwork, dungeon);
    }

    public Enemy CreateOfficialEnemy(int x, int y, Random rnd, ISubject<DeathPayload> deathNetwork, ISubject<SoundPayload> soundNetwork, Dungeon dungeon)
    {
        int los = rnd.Next(3);
        if (los == 0) return new OfficialEnemy(x, y, "Sędzia z I Ligi", 'S', 50, 10, 0, new ObronaPrzedZwyklymVisitor(), deathNetwork, soundNetwork, dungeon);
        if (los == 1) return new OfficialEnemy(x, y, "Delegat PZPN", 'D', 60, 15, 5, new ObronaPrzedSkrytymVisitor(), deathNetwork, soundNetwork, dungeon);
        return new OfficialEnemy(x, y, "Prezes Bankruta", 'B', 40, 5, 0, new ObronaPrzedZwyklymVisitor(), deathNetwork, soundNetwork, dungeon);
    }

    public Weapon CreateRandomWeapon(Random rnd)
    {
        int los = rnd.Next(3);
        if (los == 0) return new HeavyWeapon("Radovan Pankov", 'V', 50, true,5);   
        if (los == 1) return new HeavyWeapon("Kamil Piatkowski", 'P', 60, true,3); 
        return new MagicWeapon("Patryk Kun", 'K', 70, false,1);                  
    }
}