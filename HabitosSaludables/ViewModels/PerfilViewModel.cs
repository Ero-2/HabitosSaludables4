using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HabitosSaludables.Models;  // 👈 Aquí está el modelo User
using HabitosSaludables.Services;
using System.Collections.ObjectModel;

namespace HabitosSaludables.ViewModels
{
    public partial class PerfilViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;
        private User _usuarioActual;  // 👈 CAMBIADO: Usuario → User

        // ── Datos del usuario ──────────────────────────────────────────────
        [ObservableProperty]
        private string _nombreUsuario = string.Empty;

        [ObservableProperty]
        private string _email = string.Empty;

        [ObservableProperty]
        private string _iniciales = "U";

        [ObservableProperty]
        private string _nivelUsuario = "🌱 Principiante";

        // ── Estadísticas ───────────────────────────────────────────────────
        [ObservableProperty]
        private int _totalHabitos = 0;

        [ObservableProperty]
        private int _rachaMaxima = 0;

        [ObservableProperty]
        private int _diasActivo = 0;

        [ObservableProperty]
        private int _metaDiaria = 3;

        // ── Configuración ──────────────────────────────────────────────────
        [ObservableProperty]
        private bool _notificacionesActivas = true;

        [ObservableProperty]
        private bool _isLoading;

        // ── Logros ─────────────────────────────────────────────────────────
        public ObservableCollection<Logro> Logros { get; } = new();

        // ──────────────────────────────────────────────────────────────────
        public PerfilViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        // 🔄 MÉTODO PRINCIPAL: Cargar datos del usuario logueado
        public async Task CargarDatosUsuarioAsync()
        {
            IsLoading = true;
            try
            {
                // Obtener ID del usuario desde Preferences (guardado al hacer login)
                var userId = Preferences.Get("usuario_id", 0);

                if (userId > 0 && _databaseService != null)
                {
                    // 👇 CAMBIADO: GetUserById → GetUserByIdAsync (o implementar el método)
                    _usuarioActual = await _databaseService.GetUserById(userId);

                    if (_usuarioActual != null)
                    {
                        // Asignar datos reales a las propiedades
                        NombreUsuario = _usuarioActual.Name ?? "Usuario";
                        Email = _usuarioActual.Email ?? string.Empty;
                        Iniciales = ObtenerIniciales(_usuarioActual.Name);

                        // Cargar estadísticas reales
                        await CargarEstadisticasRealesAsync();
                    }
                }
                else
                {
                    // Si no hay usuario logueado, usar datos mock o redirigir
                    CargarDatosMock();
                }
            }
            catch (Exception ex)
            {
                // Logging opcional
                System.Diagnostics.Debug.WriteLine($"Error cargando perfil: {ex.Message}");
                CargarDatosMock();
            }
            finally
            {
                IsLoading = false;
            }
        }

        // 📊 Cargar estadísticas desde la base de datos
        private async Task CargarEstadisticasRealesAsync()
        {
            if (_usuarioActual == null || _databaseService == null) return;

            try
            {
                // 👇 Estos métodos deben existir en DatabaseService
                TotalHabitos = await _databaseService.GetTotalHabitosByUser(_usuarioActual.Id);
                RachaMaxima = await _databaseService.GetRachaMaxima(_usuarioActual.Id);
                DiasActivo = await _databaseService.GetDiasActivos(_usuarioActual.Id);

                // Actualizar nivel y logros con datos reales
                ActualizarNivel();
                CargarLogros();
            }
            catch
            {
                // Fallback a valores por defecto si hay error
                TotalHabitos = 0;
                RachaMaxima = 0;
                DiasActivo = 0;
                ActualizarNivel();
                CargarLogros();
            }
        }

        // 🎭 Datos mock para desarrollo/testing
        private void CargarDatosMock()
        {
            NombreUsuario = "Usuario Invitado";
            Email = "invitado@ejemplo.com";
            Iniciales = "UI";
            TotalHabitos = 0;
            RachaMaxima = 0;
            DiasActivo = 0;
            ActualizarNivel();
            CargarLogros();
        }

        // 🏆 Actualizar nivel según estadísticas
        private void ActualizarNivel()
        {
            NivelUsuario = RachaMaxima switch
            {
                >= 90 => "🏆 Leyenda",
                >= 30 => "💎 Experto",
                >= 14 => "🔥 Constante",
                >= 7 => "⭐ Comprometido",
                _ => "🌱 Principiante"
            };
        }

        // 🎖️ Cargar logros con estado real
        private void CargarLogros()
        {
            Logros.Clear();
            Logros.Add(new Logro { Titulo = "Primer día", Emoji = "🌱", Descripcion = "Completaste tu primer hábito", Desbloqueado = DiasActivo >= 1 });
            Logros.Add(new Logro { Titulo = "7 seguidos", Emoji = "🔥", Descripcion = "Racha de 7 días", Desbloqueado = RachaMaxima >= 7 });
            Logros.Add(new Logro { Titulo = "Mes perfecto", Emoji = "💎", Descripcion = "Racha de 30 días", Desbloqueado = RachaMaxima >= 30 });
            Logros.Add(new Logro { Titulo = "Coleccionista", Emoji = "📚", Descripcion = "5 hábitos activos", Desbloqueado = TotalHabitos >= 5 });
            Logros.Add(new Logro { Titulo = "Leyenda", Emoji = "🏆", Descripcion = "Racha de 90 días", Desbloqueado = RachaMaxima >= 90 });
        }

        // ── COMANDOS ───────────────────────────────────────────────────────

        [RelayCommand]
        private async Task EditarPerfilAsync()
        {
            await Shell.Current.GoToAsync("editarPerfil");
        }

        [RelayCommand]
        private async Task EditarMetaAsync()
        {
            string? result = await Shell.Current.CurrentPage.DisplayPromptAsync(
                title: "Meta diaria",
                message: "¿Cuántos hábitos quieres completar por día?",
                accept: "Guardar",
                cancel: "Cancelar",
                placeholder: MetaDiaria.ToString(),
                maxLength: 2,
                keyboard: Keyboard.Numeric,
                initialValue: MetaDiaria.ToString());

            if (int.TryParse(result, out int meta) && meta > 0 && meta <= 20)
            {
                MetaDiaria = meta;
                Preferences.Set("meta_diaria", meta);
            }
        }

        [RelayCommand]
        private async Task EditarTemaAsync()
        {
            await Shell.Current.DisplayAlert("Tema", "Próximamente disponible.", "OK");
        }

        [RelayCommand]
        private async Task ExportarDatosAsync()
        {
            bool confirmar = await Shell.Current.CurrentPage.DisplayAlert(
                "Exportar datos",
                "Se generará un archivo con tu historial de hábitos.",
                "Exportar", "Cancelar");

            if (confirmar)
                await Shell.Current.DisplayAlert("Exportar", "Función próximamente disponible.", "OK");
        }

        [RelayCommand]
        private async Task CerrarSesionAsync()
        {
            bool confirmar = await Shell.Current.CurrentPage.DisplayAlert(
                "Cerrar sesión",
                "¿Estás seguro que deseas cerrar sesión?",
                "Cerrar sesión", "Cancelar");

            if (confirmar)
            {
                // Limpiar preferences del usuario
                Preferences.Remove("usuario_id");
                Preferences.Remove("usuario_token");
                Preferences.Remove("usuario_email");

                // Redirigir a pantalla de bienvenida/login
                await Shell.Current.GoToAsync("//LoginPage");
            }
        }

        // 🔤 Helper para obtener iniciales del nombre
        public static string ObtenerIniciales(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre)) return "U";

            var partes = nombre.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (partes.Length == 0) return "U";
            if (partes.Length == 1) return partes[0][0].ToString().ToUpper();

            return $"{partes[0][0]}{partes[1][0]}".ToUpper();
        }
    }
}