namespace Gra.Map.Themes;

public interface IThemeFactory
{
    string GetIntroMessage();
    Items CreateArtefact();
    Enemy CreateEnemy(int x,int y,Random rnd);
    Weapon CreateRandomWeapon(Random rnd);
}