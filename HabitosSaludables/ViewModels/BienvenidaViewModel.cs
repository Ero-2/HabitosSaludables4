using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace HabitosSaludables.ViewModels
{
    public partial class BienvenidaViewModel : ObservableObject
    {
        [RelayCommand]
        private async Task GoToRegistro() => await Shell.Current.GoToAsync("RegistroPage");

        [RelayCommand]
        private async Task GoToLogin() => await Shell.Current.GoToAsync("LoginPage");
    }
}