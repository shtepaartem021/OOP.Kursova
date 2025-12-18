using System.Linq;
using TicTacToeGame.Core;

namespace TicTacToeGame.Data
{
    public static class UserRepository
    {
        private static readonly GameDbContext _context = new GameDbContext();

        public static void Add(User user)
        {
            if (Exists(user.Username)) throw new System.Exception("Користувач існує.");
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public static User GetByUsername(string username) =>
            _context.Users.FirstOrDefault(u => u.Username == username);

        public static bool Exists(string username) =>
            _context.Users.Any(u => u.Username == username);

        public static void SaveData() => _context.SaveChanges();

        public static void Delete(User user)
        {
            var userToRemove = GetByUsername(user.Username);
            if (userToRemove != null)
            {
                _context.Users.Remove(userToRemove);
                _context.SaveChanges();
            }
        }
    }
}