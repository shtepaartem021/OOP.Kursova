namespace TicTacToeGame.Core
{
    public abstract class Player
    {
        public string Name { get; protected set; }
        public char Symbol { get; }

        protected Player(char symbol, string name)
        {
            Symbol = symbol;
            Name = name;
        }
    }

    public class UserPlayer : Player
    {
        public User UserAccount { get; }

        public UserPlayer(char symbol, User user) : base(symbol, user.Username)
        {
            UserAccount = user;
        }
    }
    public class BotPlayer : Player
    {
        private readonly IBotStrategy _strategy;

        public BotPlayer(char symbol, string name, IBotStrategy strategy) : base(symbol, name)
        {
            _strategy = strategy;
        }

        public (int Row, int Col) MakeMove(Board board)
        {
            return _strategy.GetMove(board, Symbol);
        }
    }
}