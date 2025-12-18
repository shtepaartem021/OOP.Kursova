using System;

namespace TicTacToeGame.Core
{
    public class Board
    {
        private readonly char[,] _grid;
        public const int Size = 3;
        public const char EmptyCell = ' ';

        public Board()
        {
            _grid = new char[Size, Size];
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            for (int i = 0; i < Size; i++)
                for (int j = 0; j < Size; j++)
                    _grid[i, j] = EmptyCell;
        }

        public char GetCell(int row, int col)
        {
            if (IsOutOfBounds(row, col)) throw new ArgumentOutOfRangeException();
            return _grid[row, col];
        }

        public bool PlaceMark(int row, int col, char symbol)
        {
            if (IsOutOfBounds(row, col) || _grid[row, col] != EmptyCell)
            {
                return false;
            }

            _grid[row, col] = symbol;
            return true;
        }

        public char? GetWinner()
        {
            for (int i = 0; i < Size; i++)
            {
                if (_grid[i, 0] != EmptyCell && _grid[i, 0] == _grid[i, 1] && _grid[i, 1] == _grid[i, 2])
                    return _grid[i, 0];

                if (_grid[0, i] != EmptyCell && _grid[0, i] == _grid[1, i] && _grid[1, i] == _grid[2, i])
                    return _grid[0, i];
            }

            if (_grid[0, 0] != EmptyCell && _grid[0, 0] == _grid[1, 1] && _grid[1, 1] == _grid[2, 2])
                return _grid[0, 0];

            if (_grid[0, 2] != EmptyCell && _grid[0, 2] == _grid[1, 1] && _grid[1, 1] == _grid[2, 0])
                return _grid[0, 2];

            return null;
        }
        public bool IsFull()
        {
            foreach (var cell in _grid)
            {
                if (cell == EmptyCell) return false;
            }
            return true;
        }

        private bool IsOutOfBounds(int row, int col)
        {
            return row < 0 || row >= Size || col < 0 || col >= Size;
        }

        public void ClearCell(int row, int col)
        {
            if (!IsOutOfBounds(row, col))
            {
                _grid[row, col] = EmptyCell;
            }
        }
    }
}