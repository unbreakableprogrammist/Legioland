using Gra.Map;
using Gra.Network.DTO;

namespace Gra.View;

public interface IView
{
    // Metoda do odświeżania całego ekranu gry
    void Render(GameStateDto state, int localPlayerId, string statusMessage, bool showLogs);        
    // Ekrany początkowe i końcowe
    void ShowIntro(string introMessage);
    void ShowGameOver(string playerName, int points, int goals, string logFilePath, bool isDead);
}