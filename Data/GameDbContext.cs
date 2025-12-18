using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using TicTacToeGame.Core;

namespace TicTacToeGame.Data
{
    public class GameDbContext
    {
        private const string FilePath = "users.json";
        public List<User> Users { get; set; } = new List<User>();

        public GameDbContext()
        {
            Load();
        }

        public void Load()
        {
            if (!File.Exists(FilePath)) return;
            try
            {
                string json = File.ReadAllText(FilePath);
                Users = JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
            }
            catch { Users = new List<User>(); }
        }

        public void SaveChanges()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(Users, options);
            File.WriteAllText(FilePath, json);
        }
    }
}