namespace TicTacToeGame.UI
{
    public interface IMenuCommand
    {
        string Description { get; }
        void Execute();
    }
}