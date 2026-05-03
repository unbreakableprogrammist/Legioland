using Gra.Observer;

namespace Gra.Map.Themes;

public interface IThemeFactory
{
    string GetIntroMessage();
    Items CreateArtefact();

    Enemy CreateClubEnemy(int x, int y, Random rnd, ISubject<DeathPayload> deathNetwork,
        ISubject<SoundPayload> soundNetwork, Dungeon dungeon);

    Enemy CreateOfficialEnemy(int x, int y, Random rnd, ISubject<DeathPayload> deathNetwork,
        ISubject<SoundPayload> soundNetwork, Dungeon dungeon);
    
    Weapon CreateRandomWeapon(Random rnd);
}