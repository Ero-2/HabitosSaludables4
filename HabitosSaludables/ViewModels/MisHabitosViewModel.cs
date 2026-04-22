using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using HabitosSaludables.Models;
using HabitosSaludables.Services;
 // Asegúrate de tener creada la clase del mensaje
using System.Collections.ObjectModel;

namespace HabitosSaludables.ViewModels
{
    public partial class MisHabitosViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;

        [ObservableProperty]
        private ObservableCollection<Habito> _habitos = new();

        [ObservableProperty]
        private bool _isRefreshing;

        

        [RelayCommand]
        public async Task LoadHabitos()
        {
            try
            {
                IsRefreshing = true;

                // Reiniciar estado diario antes de cargar (opcional según tu lógica)
                await _databaseService.ResetDailyCompletions();

                var lista = await _databaseService.GetHabitosAsync();

                // Limpiamos y recargamos la colección para que la UI se entere
                Habitos.Clear();
                foreach (var h in lista)
                {
                    Habitos.Add(h);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"No se pudieron cargar los hábitos: {ex.Message}", "OK");
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        [RelayCommand]
        private async Task ToggleCompletado(Habito habito)
        {
            if (habito.CompletadoHoy)
            {
                await Shell.Current.DisplayAlert("Info", "Ya completaste este hábito hoy", "OK");
                return;
            }

            var exito = await _databaseService.ToggleCompletadoAsync(habito);
            if (exito)
            {
                // Forzamos la actualización visual en la lista
                var index = Habitos.IndexOf(habito);
                if (index != -1) Habitos[index] = habito;

                await Shell.Current.DisplayAlert("¡Felicidades!",
                    $"Completaste {habito.Nombre}\nRacha actual: {habito.RachaActual} día(s)", "OK");
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "No se pudo marcar como completado", "OK");
            }
        }

        [RelayCommand]
        private async Task EliminarHabito(Habito habito)
        {
            var confirm = await Shell.Current.DisplayAlert("Confirmar",
                $"¿Estás seguro de eliminar '{habito.Nombre}'?", "Sí", "No");

            if (confirm)
            {
                await _databaseService.DeleteHabitoAsync(habito);
                Habitos.Remove(habito);
            }
        }

        [RelayCommand]
        private async Task AgregarNuevoHabito()
        {
            // Asegúrate de que "AgregarHabitoPage" esté registrada en AppShell.xaml.cs
            await Shell.Current.GoToAsync("AgregarHabitoPage");
        }
    }
}