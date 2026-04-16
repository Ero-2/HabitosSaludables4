using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace HabitosSaludables.ViewModels
{
    public partial class HomeViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _title = "Menú Principal";

        [RelayCommand]
        private async Task CerrarSesion() => await Shell.Current.GoToAsync("//BienvenidaPage");

        [RelayCommand]
        private async Task RegistrarActividad() =>
            await Application.Current!.MainPage!.DisplayAlert("Info", "Funcionalidad en desarrollo", "OK");

        [RelayCommand]
        private async Task VerEstadisticas() =>
            await Application.Current!.MainPage!.DisplayAlert("Info", "Funcionalidad en desarrollo", "OK");

        [RelayCommand]
        private async Task Configuracion() =>
            await Application.Current!.MainPage!.DisplayAlert("Info", "Funcionalidad en desarrollo", "OK");
    }
}