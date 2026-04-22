using SQLite;
using HabitosSaludables.Models;

namespace HabitosSaludables.Services
{
    public class DatabaseService
    {
        private readonly SQLiteAsyncConnection _database;

        public DatabaseService(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Habito>().Wait();
            _database.CreateTableAsync<User>().Wait();  // 👈 Crear tabla de usuarios
        }

        // ========== MÉTODOS PARA HÁBITOS ==========
        public Task<List<Habito>> GetHabitosAsync()
            => _database.Table<Habito>().ToListAsync();

        public Task<int> SaveHabitoAsync(Habito habito)
        {
            if (habito.Id != 0)
                return _database.UpdateAsync(habito);
            else
                return _database.InsertAsync(habito);
        }

        public Task<int> DeleteHabitoAsync(Habito habito)
            => _database.DeleteAsync(habito);

        public async Task<bool> ToggleCompletadoAsync(Habito habito)
        {
            if (habito.CompletadoHoy) return false;

            var today = DateTime.Now.ToString("yyyy-MM-dd");
            var lastCheck = habito.UltimoCheck;

            if (string.IsNullOrEmpty(lastCheck))
            {
                habito.RachaActual = 1;
            }
            else
            {
                var yesterday = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                if (lastCheck == yesterday)
                    habito.RachaActual++;
                else if (lastCheck != today)
                    habito.RachaActual = 1;
            }

            if (habito.RachaActual > habito.MejorRacha)
                habito.MejorRacha = habito.RachaActual;

            habito.CompletadoHoy = true;
            habito.UltimoCheck = today;

            await _database.UpdateAsync(habito);
            return true;
        }

        public async Task ResetDailyCompletions()
        {
            var habitos = await GetHabitosAsync();
            var today = DateTime.Now.ToString("yyyy-MM-dd");
            foreach (var h in habitos)
            {
                if (h.UltimoCheck != today)
                {
                    h.CompletadoHoy = false;
                    await _database.UpdateAsync(h);
                }
            }
        }

        // ========== MÉTODOS PARA USUARIOS ==========

        // Obtener usuario por email
        public Task<User> GetUserByEmail(string email)
            => _database.Table<User>().FirstOrDefaultAsync(u => u.Email == email);

        // 👇 NUEVO: Obtener usuario por ID
        public Task<User> GetUserById(int id)
            => _database.Table<User>().FirstOrDefaultAsync(u => u.Id == id);

        // Registrar nuevo usuario
        public Task<int> RegisterUser(User user)
            => _database.InsertAsync(user);

        // 👇 NUEVO: Obtener total de hábitos por usuario
        public async Task<int> GetTotalHabitosByUser(int userId)
        {
            // Si tus hábitos tienen un campo UserId, úsalo:
            // return await _database.Table<Habito>().Where(h => h.UsuarioId == userId).CountAsync();

            // Si NO tienes campo UserId, devuelve todos los hábitos (temporal)
            return await _database.Table<Habito>().CountAsync();
        }

        // 👇 NUEVO: Obtener racha máxima del usuario
        public async Task<int> GetRachaMaxima(int userId)
        {
            // Obtener todos los hábitos del usuario
            var habitos = await _database.Table<Habito>().ToListAsync();

            // Retornar la mejor racha entre todos los hábitos
            return habitos.Any() ? habitos.Max(h => h.MejorRacha) : 0;
        }

        // 👇 NUEVO: Obtener días activos del usuario
        public async Task<int> GetDiasActivos(int userId)
        {
            // Contar hábitos que han sido completados al menos una vez
            var habitos = await _database.Table<Habito>().ToListAsync();

            // Si tienes una tabla de registros, úsala aquí
            // Por ahora, contamos hábitos con racha > 0
            return habitos.Count(h => h.RachaActual > 0 || h.UltimoCheck != null);
        }
    }
}