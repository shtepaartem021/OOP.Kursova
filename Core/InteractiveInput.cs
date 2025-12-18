using System;
using TicTacToeGame.Core;

namespace TicTacToeGame.UI
{
    public static class InteractiveInput
    {
        public static (int Row, int Col) GetMoveWithCursor(Board board, string playerName)
        {
            int cursorRow = 1;
            int cursorCol = 1;
            bool selected = false;

            while (!selected)
            {
                Console.Clear();
                Console.WriteLine($"Хід гравця: {playerName}");
                Console.WriteLine("Використовуйте СТРІЛКИ для руху, ENTER для вибору.\n");

                DrawBoardWithCursor(board, cursorRow, cursorCol);

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow: if (cursorRow > 0) cursorRow--; break;
                    case ConsoleKey.DownArrow: if (cursorRow < 2) cursorRow++; break;
                    case ConsoleKey.LeftArrow: if (cursorCol > 0) cursorCol--; break;
                    case ConsoleKey.RightArrow: if (cursorCol < 2) cursorCol++; break;
                    case ConsoleKey.Enter:
                        if (board.GetCell(cursorRow, cursorCol) == Board.EmptyCell)
                        {
                            selected = true;
                        }
                        break;
                }
            }
            return (cursorRow, cursorCol);
        }

        private static void DrawBoardWithCursor(Board board, int cursorRow, int cursorCol)
        {
            Console.WriteLine("\n  0   1   2");
            Console.WriteLine(" ┌───┬───┬───┐");
            for (int r = 0; r < 3; r++)
            {
                Console.Write($"{r}│");
                for (int c = 0; c < 3; c++)
                {
                    char symbol = board.GetCell(r, c);

                    if (r == cursorRow && c == cursorCol)
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    else
                    {
                        if (symbol == 'X') Console.ForegroundColor = ConsoleColor.Cyan;
                        else if (symbol == 'O') Console.ForegroundColor = ConsoleColor.Yellow;
                    }

                    Console.Write($" {symbol} ");
                    Console.ResetColor();
                    Console.Write("│");
                }
                Console.WriteLine();
                if (r < 2) Console.WriteLine(" ├───┼───┼───┤");
            }
            Console.WriteLine(" └───┴───┴───┘");
        }
    }
}
