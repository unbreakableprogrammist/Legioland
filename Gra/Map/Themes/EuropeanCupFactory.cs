using System;
using Gra.Map;
using Gra.Observer;

namespace Gra.Map.Themes;

public class EuropeanCupFactory : IThemeFactory
{
    public string GetIntroMessage() => 
        "Wtorki i środy na sportowo! Jedyna drużyna godnie reprezentująca Polskę w Europie wkracza na salony";

    public Items CreateArtefact() => new HeavyWeapon("Ultrasi", 'U', 250, true);
    
    public Enemy CreateClubEnemy(int x, int y, Random rnd, ISubject<DeathPayload> deathNetwork, ISubject<SoundPayload> soundNetwork, Dungeon dungeon)
    {
        int los = rnd.Next(3);
        if (los == 0) return new ThemedEnemy(x, y, "Chelsea FC", 'C', 200, 60, 60, new ObronaPrzedSkrytymVisitor(), deathNetwork, soundNetwork, dungeon);
        if (los == 1) return new ThemedEnemy(x, y, "Real Betis", 'B', 180, 50, 15, new ObronaPrzedMagicznymVisitor(), deathNetwork, soundNetwork, dungeon);
        
        return new ThemedEnemy(x, y, "Aston Villa", 'V', 190, 55, 30, new ObronaPrzedZwyklymVisitor(), deathNetwork, soundNetwork, dungeon);
    }

    public Enemy CreateOfficialEnemy(int x, int y, Random rnd, ISubject<DeathPayload> deathNetwork, ISubject<SoundPayload> soundNetwork, Dungeon dungeon)
    {
        int los = rnd.Next(3);
        if (los == 0) return new OfficialEnemy(x, y, "Sędzia UEFA", 'S', 100, 20, 5, new ObronaPrzedZwyklymVisitor(), deathNetwork, soundNetwork, dungeon);
        if (los == 1) return new OfficialEnemy(x, y, "Delegat UEFA", 'D', 80, 15, 0, new ObronaPrzedZwyklymVisitor(), deathNetwork, soundNetwork, dungeon);
        
        return new OfficialEnemy(x, y, "Sędzia VAR", 'V', 90, 10, 10, new ObronaPrzedMagicznymVisitor(), deathNetwork, soundNetwork, dungeon);
    }

    public Weapon CreateRandomWeapon(Random rnd)
    {
        int los = rnd.Next(3);
        if (los == 0) return new MagicWeapon("Juergen Elitim", 'E', 70, false,2);     
        if (los == 1) return new HeavyWeapon("Jean-Pierre Nsame", 'N', 80, true,5);   
        return new LightWeapon("Vahan Bichakhchyan", 'V', 65, false,3);               
    }
}