using System;

namespace Gra.Map.Themes;

public class MasterThemeFactory : IThemeFactory
{
    public string GetIntroMessage() => 
        "Król wraca na stolicę! Czas strącić z tronu uzurpatorów i odzyskać mistrzowski tytuł.";

    public Items CreateArtefact() => 
        new MagicWeapon("Marek Papszun", 'M', 200, true);

    public Enemy CreateEnemy(int x, int y, Random rnd)
    {
        int los = rnd.Next(3);
        if (los == 0) return new ThemedEnemy(x, y, "Lech Poznań", 'L', 150, 40, 20, new ObronaPrzedZwyklymVisitor());
        if (los == 1) return new ThemedEnemy(x, y, "Jagiellonia Białystok", 'J', 140, 35, 15, new ObronaPrzedSkrytymVisitor());
        return new ThemedEnemy(x, y, "Raków Częstochowa", 'R', 160, 1, 100, new ObronaPrzedZwyklymVisitor());
    }

    public Weapon CreateRandomWeapon(Random rnd)
    {
        int los = rnd.Next(3);
        if (los == 0) return new LightWeapon("Paweł Wszołek", 'W', 50, false);   
        if (los == 1) return new MagicWeapon("Kacper Urbański", 'U', 60, false); 
        return new HeavyWeapon("Kamil Adamski", 'A', 70, true);                  
    }
}