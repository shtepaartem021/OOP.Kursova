using System;

namespace TicTacToeGame.Core
{
    public enum GameResult
    {
        Win,
        Loss,
        Draw
    }
    public class GameRecord
    {
        public DateTime Date { get; set; }
        public string OpponentName { get; }
        public GameResult Result { get; }
        public int RatingChange { get; }

        public GameRecord(string opponentName, GameResult result, int ratingChange)
        {
            Date = DateTime.Now;
            OpponentName = opponentName;
            Result = result;
            RatingChange = ratingChange;
        }

        public override string ToString()
        {
            return $"{Date,-16:g} | Проти: {OpponentName,-8} | Результат: {Result,-4} | Рейтинг: {RatingChange:+0;-0}";
        }
    }
}