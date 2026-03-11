using System;

namespace Gra
{
    class Program
    {
        static void Main(string[] args)
        {
            Board board = new Board(20, 20);
            board.start_game();
        }
    }
}