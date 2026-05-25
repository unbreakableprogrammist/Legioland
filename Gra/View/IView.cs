using Gra.Map;
using Gra.Network.DTO;

namespace Gra.View;

public interface IView
{
    void Render(GameStateDto state, int localPlayerId, string statusMessage, bool showLogs);        
    void ShowIntro(string introMessage);
    void ShowGameOver(string playerName, int points, int goals, string logFilePath, bool isDead);
}
