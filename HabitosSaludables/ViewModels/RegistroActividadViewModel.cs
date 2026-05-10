using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HabitosSaludables.Models;
using HabitosSaludables.Services;
using System.Collections.ObjectModel;

namespace HabitosSaludables.ViewModels
{
    public partial class RegistroActividadViewModel : ObservableObject
    {
        private readonly DatabaseService _db;

        [ObservableProperty] private ObservableCollection<Habito> _habitosPendientes = new();
        [ObservableProperty] private int _completadosHoy;
        [ObservableProperty] private int _totalHabitos;

        public RegistroActividadViewModel(DatabaseService db)
        {
            _db = db;
        }

        public async Task CargarAsync()
        {
            var todos = await _db.GetHabitosAsync();
            TotalHabitos = todos.Count;
            CompletadosHoy = todos.Count(h => h.CompletadoHoy);

            HabitosPendientes.Clear();
            foreach (var h in todos.Where(h => !h.CompletadoHoy))
                HabitosPendientes.Add(h);
        }

        [RelayCommand]
        private async Task Registrar(Habito habito)
        {
            var ok = await _db.ToggleCompletadoAsync(habito);
            if (ok)
            {
                HabitosPendientes.Remove(habito);
                CompletadosHoy++;
                await Shell.Current.DisplayAlert("¡Completado!", $"{habito.Icono} {habito.Nombre}\nRacha: {habito.RachaActual} días 🔥", "OK");
            }
        }

        [RelayCommand]
        private async Task Volver() => await Shell.Current.GoToAsync("..");
    }
}
