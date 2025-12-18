using System;
using System.Collections.Generic;
using System.Linq;

namespace TicTacToeGame.Core
{
    public interface IBotStrategy
    {
        (int Row, int Col) GetMove(Board board, char botSymbol);
    }

    public class EasyBotStrategy : IBotStrategy
    {
        private readonly Random _random = new Random();

        public (int Row, int Col) GetMove(Board board, char botSymbol)
        {
            var emptyCells = GetEmptyCells(board);
            if (emptyCells.Count == 0) return (-1, -1);
            return emptyCells[_random.Next(emptyCells.Count)];
        }

        protected List<(int, int)> GetEmptyCells(Board board)
        {
            var list = new List<(int, int)>();
            for (int r = 0; r < 3; r++)
                for (int c = 0; c < 3; c++)
                    if (board.GetCell(r, c) == Board.EmptyCell) list.Add((r, c));
            return list;
        }
    }

    public class MediumBotStrategy : EasyBotStrategy
    {
        public new (int Row, int Col) GetMove(Board board, char botSymbol)
        {
            char opponentSymbol = (botSymbol == 'X') ? 'O' : 'X';

            var winMove = FindWinningMove(board, botSymbol);
            if (winMove.HasValue) return winMove.Value;

            var blockMove = FindWinningMove(board, opponentSymbol);
            if (blockMove.HasValue) return blockMove.Value;

            return base.GetMove(board, botSymbol);
        }

        private (int, int)? FindWinningMove(Board board, char symbol)
        {
            var emptyCells = GetEmptyCells(board);
            foreach (var cell in emptyCells)
            {
                board.PlaceMark(cell.Item1, cell.Item2, symbol);
                bool won = board.GetWinner() == symbol;
                if (CheckLineWin(board, cell.Item1, cell.Item2, symbol)) return cell;
                board.ClearCell(cell.Item1, cell.Item2);
            }
            return null;
        }

        private bool CheckLineWin(Board b, int r, int c, char s)
        {
            if (b.GetWinner() == s)
            {
                b.ClearCell(r, c);
                return true;
            }
            b.ClearCell(r, c);
            return false;
        }
    }

    public class HardBotStrategy : IBotStrategy
    {
        public (int Row, int Col) GetMove(Board board, char botSymbol)
        {
            char opponentSymbol = (botSymbol == 'X') ? 'O' : 'X';
            int bestScore = int.MinValue;
            (int, int) bestMove = (-1, -1);

            for (int r = 0; r < 3; r++)
            {
                for (int c = 0; c < 3; c++)
                {
                    if (board.GetCell(r, c) == Board.EmptyCell)
                    {
                        board.PlaceMark(r, c, botSymbol);
                        int score = Minimax(board, 0, false, botSymbol, opponentSymbol);
                        board.ClearCell(r, c);

                        if (score > bestScore)
                        {
                            bestScore = score;
                            bestMove = (r, c);
                        }
                    }
                }
            }
            return bestMove;
        }

        private int Minimax(Board board, int depth, bool isMaximizing, char bot, char human)
        {
            char? winner = board.GetWinner();
            if (winner == bot) return 10 - depth;
            if (winner == human) return depth - 10;
            if (board.IsFull()) return 0;

            if (isMaximizing)
            {
                int bestScore = int.MinValue;
                for (int r = 0; r < 3; r++)
                {
                    for (int c = 0; c < 3; c++)
                    {
                        if (board.GetCell(r, c) == Board.EmptyCell)
                        {
                            board.PlaceMark(r, c, bot);
                            int score = Minimax(board, depth + 1, false, bot, human);
                            board.ClearCell(r, c);
                            bestScore = Math.Max(score, bestScore);
                        }
                    }
                }
                return bestScore;
            }
            else
            {
                int bestScore = int.MaxValue;
                for (int r = 0; r < 3; r++)
                {
                    for (int c = 0; c < 3; c++)
                    {
                        if (board.GetCell(r, c) == Board.EmptyCell)
                        {
                            board.PlaceMark(r, c, human);
                            int score = Minimax(board, depth + 1, true, bot, human);
                            board.ClearCell(r, c);
                            bestScore = Math.Min(score, bestScore);
                        }
                    }
                }
                return bestScore;
            }
        }
    }
}