using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using HabitosSaludables.Services;

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
            if (string.IsNullOrWhiteSpace(_email) || string.IsNullOrWhiteSpace(_contrasena))
            {
                await Application.Current!.MainPage!.DisplayAlert("Error", "Por favor, ingresa tu correo y contraseña.", "OK");
                return;
            }

            var user = await _databaseService.GetUserByEmail(_email);
            if (user != null && user.Password == _contrasena)
            {
                await Application.Current.MainPage.DisplayAlert("Bienvenido", $"¡Hola {user.Name}!", "OK");

                // Disparar evento para mostrar popup
                if (LoginExitoso != null)
                    await LoginExitoso.Invoke();

                await Shell.Current.GoToAsync("//HomePage");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Correo o contraseña incorrectos", "OK");
            }
        }

        [RelayCommand]
        private async Task IrARegistro() => await Shell.Current.GoToAsync("RegistroPage");
    }
}