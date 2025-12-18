using System;

namespace TicTacToeGame.UI
{
    public static class InputHelper
    {
        public static int GetInt(string prompt, int min, int max)
        {
            int result;
            while (true)
            {
                Console.Write($"{prompt} ({min}-{max}): ");
                string input = Console.ReadLine();

                if (int.TryParse(input, out result) && result >= min && result <= max)
                {
                    return result;
                }
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Невірний ввід. Спробуйте ще раз.");
                Console.ResetColor();
            }
        }

        public static string GetString(string prompt)
        {
            while (true)
            {
                Console.Write($"{prompt}: ");
                string input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input)) return input;
            }
        }
    }
}