using Gra.Map;
using Gra.Network.DTO;

namespace Gra.View;
// klasa ktorej obiekt bedzie zajmowal sie rysowaniem na konsole
public class ConsoleView : IView
    {
        public void Render(GameStateDto state, int localPlayerId, string statusMessage, bool showLogs)
        {
            if (showLogs)
            {
                // Logi są teraz w obiekcie state.Logs
                DrawLogsWindow(state.Logs);
            }
            else
            {
                DrawNormalGame(state, localPlayerId, statusMessage);
            }
        }
        
        private void DrawLogsWindow(List<string> logs)
        {
            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("=== DZIENNIK ZDARZEŃ LEGIOLANDU (Wciśnij J, aby wrócić) ===");
            Console.ResetColor();
            Console.WriteLine("------------------------------------------------------------");

            int start = Math.Max(0, logs.Count - 20);
            for (int i = start; i < logs.Count; i++)
            {
                Console.WriteLine(logs[i]);
            }
        }

        private void DrawNormalGame(GameStateDto state, int localPlayerId, string statusMessage)
        {
            // Znajdujemy gracza, w którego się wcielamy (dla UI)
            PlayerDto myPlayer = state.Players.FirstOrDefault(p => p.Id == localPlayerId);
            if (myPlayer == null) return; // Zapobiega błędom, gdy gracza nie ma

            Console.SetCursorPosition(0, 0);
            
            // 1. RYSOWANIE MAPY NA PODSTAWIE DTO
            for (int y = 0; y < state.MapHeight; y++)
            {
                for (int x = 0; x < state.MapWidth; x++)
                {
                    // Sprawdzamy czy na tym polu jest jakikolwiek gracz
                    PlayerDto playerOnTile = state.Players.FirstOrDefault(p => p.X == x && p.Y == y);
                    // Sprawdzamy czy na tym polu jest przeciwnik
                    EnemyDto enemyOnTile = state.Enemies.FirstOrDefault(e => e.X == x && e.Y == y);

                    if (playerOnTile != null)
                    {
                        // Nasz gracz jest Cyan, inni gracze np. Żółci
                        Console.ForegroundColor = playerOnTile.Id == localPlayerId ? ConsoleColor.Cyan : ConsoleColor.Yellow;
                        Console.Write(playerOnTile.Symbol);
                        Console.ResetColor();
                    }
                    else if (enemyOnTile != null)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(enemyOnTile.Symbol);
                        Console.ResetColor();
                    }
                    else
                    {
                        char symbol = state.Map[x][y].Symbol;
                        if (symbol == '#') Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(symbol);
                        Console.ResetColor();
                    }
                }
                Console.WriteLine();
            }

            // 2. RYSOWANIE UI NA PODSTAWIE DTO
            int uiColumn = state.MapWidth + 20;
            int clearWidth = 60;

            Console.SetCursorPosition(uiColumn, 0);

            if (myPlayer.IsInCombatMode)
            {
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("=== T R Y B   W A L K I ===".PadRight(clearWidth));
                Console.ResetColor();
            }
            else
            {
                Console.Write("=== LEGIOLAND (MAPA) ===".PadRight(clearWidth));
            }

            // Wyciąganie itemów na ziemi z DTO
            var itemsOnGround = state.Map[myPlayer.X][myPlayer.Y].Items;

            for (int i = 0; i < Math.Max(5, itemsOnGround.Count); i++)
            {
                Console.SetCursorPosition(uiColumn, i + 1);
                if (i < itemsOnGround.Count)
                {
                    string prefix = (i == myPlayer.SelectedGroundSlot) ? "-> " : "   ";
                    Console.Write($"{prefix}{itemsOnGround[i]} [Press E]".PadRight(clearWidth));
                }
                else Console.Write("".PadRight(clearWidth));
            }

            Console.SetCursorPosition(uiColumn, 7);
            Console.Write($"PUNKTY: {myPlayer.Points}  GOLE: {myPlayer.Goals}".PadRight(clearWidth));

            Console.SetCursorPosition(uiColumn, 8);
            Console.Write($"HP: {myPlayer.Health}   STRENGHT : {myPlayer.Strength}".PadRight(clearWidth));

            Console.SetCursorPosition(uiColumn, 9);
            Console.Write($"Luck: {myPlayer.TotalLuck}  Wisdom: {myPlayer.Wisdom}".PadRight(clearWidth));

            Console.SetCursorPosition(uiColumn, 10);
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write($"STYL ATAKU: {myPlayer.AttackStyleName}".PadRight(clearWidth));
            Console.ResetColor();

            Console.SetCursorPosition(uiColumn, 11);
            EnemyDto target = state.Enemies.FirstOrDefault(e => e.X == myPlayer.X && e.Y == myPlayer.Y);

            if (target != null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                string trybInfo = myPlayer.IsInCombatMode ? "[WALKA]" : "[MOZLIWE]";
                Console.Write($"{trybInfo} CEL: {target.Name} (HP: {target.Health} | Atk: {target.BaseDamage})".PadRight(clearWidth));
                Console.ResetColor();
            }
            else
            {
                Console.Write("".PadRight(clearWidth));
            }

            Console.SetCursorPosition(uiColumn, 13);
            Console.Write($"LEWA (L): {myPlayer.LeftHandName}".PadRight(clearWidth));

            Console.SetCursorPosition(uiColumn, 14);
            Console.Write($"PRAWA (R): {myPlayer.RightHandName}".PadRight(clearWidth));

            Console.SetCursorPosition(uiColumn, 16);
            Console.Write("=== SKŁAD (PLECAK) ===".PadRight(clearWidth));
            
            for (int i = 0; i < Math.Max(10, myPlayer.BackpackNames.Count); i++)
            {
                Console.SetCursorPosition(uiColumn, i + 17);
                if (i < myPlayer.BackpackNames.Count)
                {
                    string prefix = (i == myPlayer.SelectedInventorySlot) ? "-> " : "   ";
                    Console.Write($"{prefix}{myPlayer.BackpackNames[i]}".PadRight(clearWidth));
                }
                else Console.Write("".PadRight(clearWidth));
            }

            Console.SetCursorPosition(0, state.MapHeight + 2);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(statusMessage.PadRight(uiColumn - 2));
            Console.ResetColor();
        }
    public void ShowIntro(string introMessage)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Green;

        if (File.Exists("Intro.txt"))
        {
            string logo = File.ReadAllText("Intro.txt");
            Console.WriteLine(logo);
        }
        else
        {
            Console.WriteLine("Brak pliku Intro.txt! Upewnij się, że jest w folderze z grą (Copy to Output Directory).");
        }
        Console.ResetColor();

        Console.WriteLine("\n\t\t\t [ NACIŚNIJ ENTER, ABY ROZPOCZĄĆ PRZYGODĘ ]");

        while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
        Console.Clear();

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n\t\tDAWNO TEMU W ODLEGŁEJ GALAKTYCE... A MOŻE PRZY ŁAZIENKOWSKIEJ...\n\n");
        
        foreach (char c in introMessage)
        {
            Console.Write(c);
            if (c != ' ')
            {
                Thread.Sleep(30);
            }
            if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Spacebar)
            {
                Console.Clear();
                Console.WriteLine("\n\t\tDAWNO TEMU W ODLEGŁEJ GALAKTYCE... A MOŻE PRZY ŁAZIENKOWSKIEJ...\n\n");
                Console.WriteLine(introMessage);
                break;
            }
        }

        Console.ResetColor();

        Console.WriteLine("\n\nNaciśnij dowolny klawisz, aby zobaczyć sterowanie...");
        Console.ReadKey(true);

        Console.Clear();
        Console.WriteLine("=== INSTRUKCJA TAKTYCZNA (STEROWANIE) ===");
        Console.WriteLine("\n[W, A, S, D]      - Poruszanie się po mapie");
        Console.WriteLine("[STRZAŁKI </>]    - Wybieranie przedmiotów na ziemi");
        Console.WriteLine("""[STRZAŁKI /|\ / \|/ ]    - Przeglądanie plecaka""");
        Console.WriteLine("[E]               - Podniesienie przedmiotu");
        Console.WriteLine("[F]               - Wyrzucenie przedmiotu z plecaka");
        Console.WriteLine("[L]               - Wyposażenie w lewą rękę");
        Console.WriteLine("[R]               - Wyposażenie w prawą rękę");
        Console.WriteLine("[1, 2, 3]         - Zmiana stylu walki (Zwykły/Skryty/Magiczny)");
        Console.WriteLine("[X]               - Wejście/Wyjście z trybu walki");
        Console.WriteLine("[Q]               - Poddanie meczu (Wyjście)");

        Console.WriteLine("\n\nWszystko jasne? Ruszajmy na boisko!");
        Console.WriteLine("Naciśnij dowolny klawisz, aby wybiec z tunelu...");
        Console.ReadKey(true);
    }

    public void ShowGameOver(string playerName, int points, int goals, string logFilePath, bool isDead)
    {
        Console.CursorVisible = true;
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Red;
        if (File.Exists("end.txt"))
        {
            Console.WriteLine(File.ReadAllText("end.txt"));
        }
        else
        {
            Console.WriteLine("======================================");
            Console.WriteLine("          G A M E   O V E R  " + "         ");
            Console.WriteLine("======================================");
            Console.WriteLine("Twoja przygoda na Legiolandzie dobiegła końca...");
        }

        Console.ResetColor();
        Console.WriteLine("\n--- PODSUMOWANIE SEZONU ---");
        Console.WriteLine($"Zawodnik: {playerName}");
        Console.WriteLine($"Punkty: {points}");
        Console.WriteLine($"Gole: {goals}");

        Console.WriteLine($"\n[Protokół meczowy (logi) został zapisany w: {logFilePath}]");
        Console.WriteLine("\nNaciśnij dowolny klawisz, aby wyjść do szatni...");
        Console.ReadKey(true);
    }
}