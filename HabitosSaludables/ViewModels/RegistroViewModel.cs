using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HabitosSaludables.Models;
using HabitosSaludables.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HabitosSaludables.ViewModels
{
    public partial class RegistroViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;

        [ObservableProperty]
        private string _nombre = string.Empty;

        [ObservableProperty]
        private string _email = string.Empty;

        [ObservableProperty]
        private string _contrasena = string.Empty;

        public RegistroViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        [RelayCommand]
        private async Task Registrar()
        {
            if (string.IsNullOrWhiteSpace(_nombre) || string.IsNullOrWhiteSpace(_email) || string.IsNullOrWhiteSpace(_contrasena))
            {
                await Application.Current!.MainPage!.DisplayAlert("Error", "Todos los campos son obligatorios.", "OK");
                return;
            }

            var existingUser = await _databaseService.GetUserByEmail(_email);
            if (existingUser != null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "El correo ya está registrado.", "OK");
                return;
            }

            var user = new User
            {
                Name = _nombre,
                Email = _email,
                Password = _contrasena
            };

            await _databaseService.RegisterUser(user);
            await Application.Current.MainPage.DisplayAlert("Éxito", "Usuario registrado correctamente", "OK");
            await Shell.Current.GoToAsync("//LoginPage");
        }

        [RelayCommand]
        private async Task IrALogin() => await Shell.Current.GoToAsync("LoginPage");
    }
}