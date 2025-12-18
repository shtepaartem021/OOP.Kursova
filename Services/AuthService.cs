using TicTacToeGame.Core;
using TicTacToeGame.Data;

namespace TicTacToeGame.Services
{
    public class AuthService
    {
        public User CurrentUser { get; private set; }

        public string Register(string username, string password)
        {
            if (UserRepository.Exists(username))
            {
                return "Помилка: Користувач вже існує.";
            }

            try
            {
                User newUser = new User(username, password);
                UserRepository.Add(newUser);
                return "Успіх: Користувач зареєстрований.";
            }
            catch (System.Exception ex)
            {
                return $"Помилка валідації: {ex.Message}";
            }
        }

        public bool Login(string username, string password)
        {
            var user = UserRepository.GetByUsername(username);

            if (user != null && user.CheckPassword(password))
            {
                CurrentUser = user;
                return true;
            }
            return false;
        }

        public void Logout()
        {
            CurrentUser = null;
        }
        public void DeleteAccount()
        {
            if (CurrentUser != null)
            {
                UserRepository.Delete(CurrentUser);
                CurrentUser = null;
            }
        }
    }
  }