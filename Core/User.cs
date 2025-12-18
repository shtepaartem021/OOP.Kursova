using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TicTacToeGame.Core
{
    public class User
    {
        public string Username { get; set; }
        public int Rating { get; set; }

        public string PasswordHash { get; set; }
        public List<GameRecord> GameHistory { get; set; } = new List<GameRecord>();

        public User() { }

        public User(string username, string password)
        {
            Username = username;
            SetPassword(password);
            Rating = 1000;
        }

        public bool CheckPassword(string password)
        {
            return PasswordHash == password;
        }

        private void SetPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < 4)
                throw new System.ArgumentException("Пароль занадто короткий!");

            PasswordHash = password;
        }

        public void CompleteGame(string opponentName, GameResult result)
        {
            int pointsChanged = 0;
            switch (result)
            {
                case GameResult.Win: pointsChanged = 25; break;
                case GameResult.Loss: pointsChanged = -25; break;
                case GameResult.Draw: pointsChanged = 5; break;
            }

            if (Rating + pointsChanged < 0) Rating = 0;
            else Rating += pointsChanged;

            GameHistory.Add(new GameRecord(opponentName, result, pointsChanged));
        }
    }
}