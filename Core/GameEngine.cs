using System;

namespace TicTacToeGame.Core
{
    public class GameEngine
    {
        public Board Board { get; }
        public Player PlayerX { get; }
        public Player PlayerO { get; }
        public Player CurrentPlayer { get; private set; }
        public bool IsGameOver { get; private set; }
        public string StatusMessage { get; private set; }

        public GameEngine(Player playerX, Player playerO)
        {
            Board = new Board();
            PlayerX = playerX;
            PlayerO = playerO;
            CurrentPlayer = PlayerX;
            IsGameOver = false;
            StatusMessage = $"Хід гравця: {CurrentPlayer.Name} ({CurrentPlayer.Symbol})";
        }

        public bool MakeMove(int row, int col)
        {
            if (IsGameOver)
            {
                StatusMessage = "Гра вже завершена.";
                return false;
            }

            if (!Board.PlaceMark(row, col, CurrentPlayer.Symbol))
            {
                StatusMessage = "Недопустимий хід! Клітинка зайнята або поза межами.";
                return false;
            }

            if (CheckGameEnd())
            {
                return true;
            }

            SwitchTurn();
            return true;
        }

        private void SwitchTurn()
        {
            CurrentPlayer = (CurrentPlayer == PlayerX) ? PlayerO : PlayerX;
            StatusMessage = $"Хід гравця: {CurrentPlayer.Name} ({CurrentPlayer.Symbol})";
        }

        private bool CheckGameEnd()
        {
            char? winnerSymbol = Board.GetWinner();

            if (winnerSymbol.HasValue)
            {
                IsGameOver = true;
                Player winner = (winnerSymbol == PlayerX.Symbol) ? PlayerX : PlayerO;
                Player loser = (winner == PlayerX) ? PlayerO : PlayerX;

                StatusMessage = $"Гру завершено! Виграв {winner.Name} ({winner.Symbol})";

                UpdateStats(winner, GameResult.Win, loser.Name);
                UpdateStats(loser, GameResult.Loss, winner.Name);

                return true;
            }

            if (Board.IsFull())
            {
                IsGameOver = true;
                StatusMessage = "Нічия!";

                UpdateStats(PlayerX, GameResult.Draw, PlayerO.Name);
                UpdateStats(PlayerO, GameResult.Draw, PlayerX.Name);

                return true;
            }

            return false;
        }
        private void UpdateStats(Player player, GameResult result, string opponentName)
        {
            if (player is UserPlayer userPlayer)
            {
                userPlayer.UserAccount.CompleteGame(opponentName, result);
                Console.WriteLine($"[LOG] Дані користувача {userPlayer.Name} оновлено.");
            }
        }
    }
}