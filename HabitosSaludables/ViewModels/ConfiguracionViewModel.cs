using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace HabitosSaludables.ViewModels
{
    public partial class ConfiguracionViewModel : ObservableObject
    {
        [ObservableProperty] private bool _notificacionesActivas;
        [ObservableProperty] private int _metaDiaria;

        public ConfiguracionViewModel()
        {
            _notificacionesActivas = Preferences.Get("notificaciones", true);
            _metaDiaria = Preferences.Get("meta_diaria", 3);
        }

        partial void OnNotificacionesActivasChanged(bool value)
            => Preferences.Set("notificaciones", value);

        [RelayCommand]
        private async Task EditarMeta()
        {
            string? result = await Shell.Current.CurrentPage.DisplayPromptAsync(
                "Meta diaria",
                "¿Cuántos hábitos quieres completar por día?",
                "Guardar", "Cancelar",
                MetaDiaria.ToString(), 2, Keyboard.Numeric, MetaDiaria.ToString());

            if (int.TryParse(result, out int meta) && meta > 0 && meta <= 20)
            {
                MetaDiaria = meta;
                Preferences.Set("meta_diaria", meta);
            }
        }

        [RelayCommand]
        private async Task CerrarSesion()
        {
            bool ok = await Shell.Current.CurrentPage.DisplayAlert(
                "Cerrar sesión", "¿Estás seguro que deseas salir?", "Cerrar sesión", "Cancelar");

            if (ok)
            {
                Preferences.Remove("usuario_id");
                Preferences.Remove("usuario_token");
                Preferences.Remove("usuario_email");
                await Shell.Current.GoToAsync("//LoginPage");
            }
        }

        [RelayCommand]
        private async Task Volver() => await Shell.Current.GoToAsync("..");
    }
}
