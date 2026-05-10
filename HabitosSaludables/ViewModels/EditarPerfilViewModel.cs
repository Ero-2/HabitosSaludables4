using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HabitosSaludables.Services;

namespace HabitosSaludables.ViewModels
{
    public partial class EditarPerfilViewModel : ObservableObject
    {
        private readonly DatabaseService _db;

        [ObservableProperty] private string _contrasenaActual = string.Empty;
        [ObservableProperty] private string _nuevaContrasena = string.Empty;
        [ObservableProperty] private string _confirmarContrasena = string.Empty;
        [ObservableProperty] private bool _modoOscuro;

        public EditarPerfilViewModel(DatabaseService db)
        {
            _db = db;
            _modoOscuro = Preferences.Get("dark_mode", false);
        }

        partial void OnModoOscuroChanged(bool value)
        {
            Preferences.Set("dark_mode", value);
            if (Application.Current != null)
                Application.Current.UserAppTheme = value ? AppTheme.Dark : AppTheme.Light;
        }

        [RelayCommand]
        private async Task CambiarContrasena()
        {
            if (string.IsNullOrWhiteSpace(ContrasenaActual) ||
                string.IsNullOrWhiteSpace(NuevaContrasena) ||
                string.IsNullOrWhiteSpace(ConfirmarContrasena))
            {
                await Shell.Current.DisplayAlert("Error", "Completa todos los campos.", "OK");
                return;
            }

            if (NuevaContrasena != ConfirmarContrasena)
            {
                await Shell.Current.DisplayAlert("Error", "Las contraseñas nuevas no coinciden.", "OK");
                return;
            }

            if (NuevaContrasena.Length < 6)
            {
                await Shell.Current.DisplayAlert("Error", "La contraseña debe tener al menos 6 caracteres.", "OK");
                return;
            }

            var userId = Preferences.Get("usuario_id", 0);
            if (userId == 0)
            {
                await Shell.Current.DisplayAlert("Error", "Sesión no encontrada.", "OK");
                return;
            }

            var user = await _db.GetUserById(userId);
            if (user == null)
            {
                await Shell.Current.DisplayAlert("Error", "Usuario no encontrado.", "OK");
                return;
            }

            if (user.Password != ContrasenaActual)
            {
                await Shell.Current.DisplayAlert("Error", "La contraseña actual es incorrecta.", "OK");
                return;
            }

            user.Password = NuevaContrasena;
            await _db.UpdateUserAsync(user);

            ContrasenaActual = string.Empty;
            NuevaContrasena = string.Empty;
            ConfirmarContrasena = string.Empty;

            await Shell.Current.DisplayAlert("¡Listo!", "Contraseña actualizada correctamente.", "OK");
        }

        [RelayCommand]
        private async Task Volver() => await Shell.Current.GoToAsync("..");
    }
}
