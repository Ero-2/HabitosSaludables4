using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using HabitosSaludables.Models;
using HabitosSaludables.Services;
using HabitosSaludables.Views;
using System.Collections.ObjectModel;

namespace HabitosSaludables.ViewModels
{
    public partial class HomeViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;

        [ObservableProperty]
        private string _title = "Menú Principal";

        [ObservableProperty]
        private string _nombreUsuario = "Usuario";

        [ObservableProperty]
        private int _rachaTotal = 0;  // Racha total del usuario (suma de rachas actuales de todos los hábitos)

        [ObservableProperty]
        private ObservableCollection<Habito> _habitosPendientes = new();

        public HomeViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
            CargarHabitosCommand.Execute(null);
        }

        [RelayCommand]
        private async Task CargarHabitos()
        {
            var todos = await _databaseService.GetHabitosAsync();
            // Mostrar solo hábitos no completados hoy, pero también se pueden mostrar todos
            var pendientes = todos.Where(h => !h.CompletadoHoy).ToList();
            HabitosPendientes.Clear();
            foreach (var h in pendientes)
                HabitosPendientes.Add(h);

            // Calcular racha total (suma de rachas actuales)
            RachaTotal = todos.Sum(h => h.RachaActual);
        }

        [RelayCommand]
        private async Task MarcarCompletado(Habito habito)
        {
            if (habito.CompletadoHoy)
            {
                await Shell.Current.DisplayAlert("Info", "Ya completaste este hábito hoy", "OK");
                return;
            }

            var exito = await _databaseService.ToggleCompletadoAsync(habito);
            if (exito)
            {
                // Eliminar de la lista pendiente
                HabitosPendientes.Remove(habito);
                // Actualizar racha total
                RachaTotal += 1; // Porque al marcarlo, su racha actual aumentó en 1
                await Shell.Current.DisplayAlert("¡Felicidades!",
                    $"Completaste {habito.Nombre}\nNueva racha: {habito.RachaActual} días", "OK");
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "No se pudo marcar", "OK");
            }
        }

        [RelayCommand]
        private async Task CerrarSesion() => await Shell.Current.GoToAsync("//BienvenidaPage");

        [RelayCommand]
        private async Task RegistrarActividad() => await Shell.Current.GoToAsync(nameof(RegistroActividadPage));

        [RelayCommand]
        private async Task VerEstadisticas() => await Shell.Current.GoToAsync(nameof(EstadisticasPage));

        [RelayCommand]
        private async Task Configuracion() => await Shell.Current.GoToAsync(nameof(ConfiguracionPage));
    }
}