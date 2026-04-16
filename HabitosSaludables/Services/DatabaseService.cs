using HabitosSaludables.Models;
using SQLite;

namespace HabitosSaludables.Services
{
    public class DatabaseService
    {
        private readonly SQLiteAsyncConnection _database;

        public DatabaseService()
        {
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "HabitosSaludables.db");
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<User>().Wait();
        }

        public Task<int> RegisterUser(User user) => _database.InsertAsync(user);

        public Task<User> GetUserByEmail(string email) =>
            _database.Table<User>().Where(u => u.Email == email).FirstOrDefaultAsync();
    }
}