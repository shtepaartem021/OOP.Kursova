using System;
using TicTacToeGame.UI;

namespace TicTacToeGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.Title = "Tic-Tac-Toe: OOP Edition";

            GameMenu menu = new GameMenu();

            try
            {
                menu.Run();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nСталася критична помилка:");
                Console.WriteLine(ex.Message);
                Console.ResetColor();
                Console.WriteLine("Натисніть будь-яку клавішу для виходу...");
                Console.ReadKey();
            }
        }
    }
}