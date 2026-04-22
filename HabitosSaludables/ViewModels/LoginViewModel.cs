using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using HabitosSaludables.Services;
using HabitosSaludables.Models;

namespace HabitosSaludables.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;

        [ObservableProperty]
        private string _email = string.Empty;

        [ObservableProperty]
        private string _contrasena = string.Empty;

        // Evento para notificar login exitoso (vista mostrará popup)
        public event Func<Task>? LoginExitoso;

        public LoginViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        [RelayCommand]
        private async Task IniciarSesion()
        {
            // 🔹 Validación de campos vacíos
            if (string.IsNullOrWhiteSpace(_email) || string.IsNullOrWhiteSpace(_contrasena))
            {
                await Application.Current!.MainPage!.DisplayAlert(
                    "Error",
                    "Por favor, ingresa tu correo y contraseña.",
                    "OK");
                return;
            }

            // 🔹 Buscar usuario en la base de datos
            var user = await _databaseService.GetUserByEmail(_email);

            // 🔹 Verificar credenciales
            if (user != null && user.Password == _contrasena)
            {
                // 🎉 LOGIN EXITOSO - Guardar datos del usuario en Preferences
                await GuardarSesionUsuarioAsync(user);

                // Mostrar mensaje de bienvenida
                await Application.Current.MainPage.DisplayAlert(
                    "Bienvenido",
                    $"¡Hola {user.Name}!",
                    "OK");

                // Disparar evento para que la vista muestre popup adicional si es necesario
                if (LoginExitoso != null)
                    await LoginExitoso.Invoke();

                // 🔹 Navegar a la página principal (HomePage)
                // El doble slash "//" asegura que se navegue a la raíz del Shell
                await Shell.Current.GoToAsync("//HomePage");
            }
            else
            {
                // ❌ Credenciales incorrectas
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Correo o contraseña incorrectos",
                    "OK");
            }
        }

        // 🔐 MÉTODO: Guardar sesión del usuario en Preferences
        private async Task GuardarSesionUsuarioAsync(User user)
        {
            // Guardar ID del usuario (CRÍTICO para que PerfilViewModel cargue datos reales)
            Preferences.Set("usuario_id", user.Id);

            // Guardar email para referencia rápida
            Preferences.Set("usuario_email", user.Email);

            // Guardar nombre para mostrar en UI sin consultar DB constantemente
            Preferences.Set("usuario_nombre", user.Name);

            // Generar y guardar un token simple de sesión (puedes mejorar esto con JWT después)
            var token = $"{user.Id}_{Guid.NewGuid():N}";
            Preferences.Set("usuario_token", token);

            // Guardar timestamp de último login
            Preferences.Set("usuario_last_login", DateTime.Now.ToString("o"));

            // Esperar un poco para asegurar que se guardó todo (opcional pero recomendado)
            await Task.Delay(50);
        }

        // 🔹 Comando para navegar a la página de registro
        [RelayCommand]
        private async Task IrARegistro() => await Shell.Current.GoToAsync("RegistroPage");

        // 🔹 Método opcional: Verificar si hay sesión activa al iniciar la app
        public static bool HaySesionActiva()
        {
            var userId = Preferences.Get("usuario_id", 0);
            var token = Preferences.Get("usuario_token", string.Empty);

            return userId > 0 && !string.IsNullOrWhiteSpace(token);
        }

        // 🔹 Método opcional: Cerrar sesión (también disponible en PerfilViewModel)
        public static async Task CerrarSesionAsync()
        {
            // Eliminar todas las preferencias del usuario
            Preferences.Remove("usuario_id");
            Preferences.Remove("usuario_email");
            Preferences.Remove("usuario_nombre");
            Preferences.Remove("usuario_token");
            Preferences.Remove("usuario_last_login");

            // Navegar a la pantalla de login
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}