using System;
using System.Collections.Generic;
using TicTacToeGame.Core;
using TicTacToeGame.Services;
using TicTacToeGame.Data;

namespace TicTacToeGame.UI
{
    public class GameMenu
    {
        private readonly AuthService _authService;
        private bool _isProgramRunning = true;

        public GameMenu()
        {
            _authService = new AuthService();
        }

        public void Run()
        {
            while (_isProgramRunning)
            {
                Console.Clear();
                Console.WriteLine("ХРЕСТИКИ-НУЛИКИ");

                if (_authService.CurrentUser == null)
                    HandleMenu(GetGuestCommands());
                else
                    HandleMenu(GetUserCommands());
            }
        }

        private void HandleMenu(List<IMenuCommand> commands)
        {
            for (int i = 0; i < commands.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {commands[i].Description}");
            }

            int choice = InputHelper.GetInt("Оберіть дію", 1, commands.Count);
            commands[choice - 1].Execute();
        }

        private List<IMenuCommand> GetGuestCommands()
        {
            return new List<IMenuCommand>
            {
                new MenuAction("Вхід", Login),
                new MenuAction("Реєстрація", Register),
                new MenuAction("Вихід", () => _isProgramRunning = false)
            };
        }

        private List<IMenuCommand> GetUserCommands()
        {
            var user = _authService.CurrentUser;
            if (user != null)
                Console.WriteLine($"Вітаємо, {user.Username}! (Рейтинг: {user.Rating})");

            return new List<IMenuCommand>
            {
                new MenuAction("Грати проти Бота", SetupGame),
                new MenuAction("Статистика та Історія", ShowStats),
                new MenuAction("Видалити акаунт", DeleteAccount),
                new MenuAction("Вихід з акаунту", () => _authService.Logout())
            };
        }

        private void SetupGame()
        {
            Console.Clear();
            Console.WriteLine("Оберіть складність бота:");
            Console.WriteLine("1. Легкий");
            Console.WriteLine("2. Середній");
            Console.WriteLine("1. Складний");
            int diff = InputHelper.GetInt("Ваш вибір", 1, 3);

            IBotStrategy strategy = diff switch
            {
                1 => new EasyBotStrategy(),
                2 => new MediumBotStrategy(),
                3 => new HardBotStrategy(),
            };

            string botName = diff switch
            {
                1 => "EasyBot",
                2 => "MedBot",
                3 => "HardBot",
            };

            Console.WriteLine("\nОберіть сторону:");
            Console.WriteLine("1. Грати за Х (Перший хід)");
            Console.WriteLine("2. Грати за О (Другий хід)");
            int side = InputHelper.GetInt("Ваш вибір", 1, 2);

            char userSymbol = (side == 1) ? 'X' : 'O';
            char botSymbol = (side == 1) ? 'O' : 'X';

            Player userPlayer = new UserPlayer(userSymbol, _authService.CurrentUser);
            Player botPlayer = new BotPlayer(botSymbol, botName, strategy);

            Player playerX = (side == 1) ? userPlayer : botPlayer;
            Player playerO = (side == 1) ? botPlayer : userPlayer;

            StartGameLoop(playerX, playerO);
        }

        private void StartGameLoop(Player pX, Player pO)
        {
            GameEngine engine = new GameEngine(pX, pO);

            while (!engine.IsGameOver)
            {
                Console.Clear();
                Console.WriteLine($"Гра: {pX.Name} (X) vs {pO.Name} (O)");
                BoardRenderer.Draw(engine.Board);
                Console.WriteLine(engine.StatusMessage);

                Player current = engine.CurrentPlayer;
                if (current is BotPlayer bot)
                {
                    Console.WriteLine("Бот думає...");
                    System.Threading.Thread.Sleep(600);
                    var move = bot.MakeMove(engine.Board);
                    engine.MakeMove(move.Row, move.Col);
                }
                else
                {
                    var move = InteractiveInput.GetMoveWithCursor(engine.Board, current.Name);
                    engine.MakeMove(move.Row, move.Col);
                }
            }

            Console.Clear();
            BoardRenderer.Draw(engine.Board);
            Console.WriteLine("=== ГРУ ЗАВЕРШЕНО ===");
            Console.WriteLine(engine.StatusMessage);

            UserRepository.SaveData();
            Console.WriteLine("\nНатисніть Enter для повернення в меню...");
            Console.ReadLine();
        }

        private void Login()
        {
            string n = InputHelper.GetString("Логін");
            string p = InputHelper.GetString("Пароль");
            if (_authService.Login(n, p)) Console.WriteLine("Вхід успішний!");
            else Console.WriteLine("Помилка входу.");
            Console.ReadKey();
        }

        private void Register()
        {
            string n = InputHelper.GetString("Логін");
            string p = InputHelper.GetString("Пароль (4+)");
            Console.WriteLine(_authService.Register(n, p));
            Console.ReadKey();
        }

        private void DeleteAccount()
        {
            Console.Write("Ви впевнені, що хочете видалити акаунт? (y/n): ");
            if (Console.ReadLine()?.ToLower() == "y")
            {
                _authService.DeleteAccount();
                Console.WriteLine("Акаунт видалено.");
                Console.ReadKey();
            }
        }

        private void ShowStats()
        {
            var u = _authService.CurrentUser;
            Console.Clear();
            Console.WriteLine($"Користувач: {u.Username} | Рейтинг: {u.Rating}");
            if (u.GameHistory != null)
                foreach (var r in u.GameHistory) Console.WriteLine(r.ToString());
            Console.ReadKey();
        }

        private class MenuAction : IMenuCommand
        {
            public string Description { get; }
            private readonly Action _act;
            public MenuAction(string d, Action a) { Description = d; _act = a; }
            public void Execute() => _act();
        }
    }
}