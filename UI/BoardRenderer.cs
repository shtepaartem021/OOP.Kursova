using System;
using TicTacToeGame.Core;

namespace TicTacToeGame.UI
{
    public static class BoardRenderer
    {
        public static void Draw(Board board)
        {
            Console.WriteLine("\n  0   1   2");
            Console.WriteLine(" ┌───┬───┬───┐");

            for (int r = 0; r < 3; r++)
            {
                Console.Write($"{r}│");
                for (int c = 0; c < 3; c++)
                {
                    char symbol = board.GetCell(r, c);
                    SetColorForSymbol(symbol);
                    Console.Write($" {symbol} ");
                    Console.ResetColor();
                    Console.Write("│");
                }
                Console.WriteLine();
                if (r < 2) Console.WriteLine(" ├───┼───┼───┤");
            }
            Console.WriteLine(" └───┴───┴───┘\n");
        }

        private static void SetColorForSymbol(char symbol)
        {
            if (symbol == 'X') Console.ForegroundColor = ConsoleColor.Cyan;
            else if (symbol == 'O') Console.ForegroundColor = ConsoleColor.Yellow;
        }
    }
}