using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HabitosSaludables.Models;
using HabitosSaludables.Services;
using System.Collections.ObjectModel;

namespace HabitosSaludables.ViewModels
{
    public partial class EstadisticasViewModel : ObservableObject
    {
        private readonly DatabaseService _db;

        [ObservableProperty] private int _totalHabitos;
        [ObservableProperty] private int _rachaMaxima;
        [ObservableProperty] private int _diasActivos;
        [ObservableProperty] private int _completadosHoy;
        [ObservableProperty] private ObservableCollection<Habito> _habitos = new();

        public EstadisticasViewModel(DatabaseService db)
        {
            _db = db;
        }

        public async Task CargarAsync()
        {
            var lista = await _db.GetHabitosAsync();
            TotalHabitos = lista.Count;
            RachaMaxima = lista.Any() ? lista.Max(h => h.MejorRacha) : 0;
            DiasActivos = lista.Count(h => h.RachaActual > 0 || h.UltimoCheck != null);
            CompletadosHoy = lista.Count(h => h.CompletadoHoy);

            Habitos.Clear();
            foreach (var h in lista.OrderByDescending(h => h.MejorRacha))
                Habitos.Add(h);
        }

        [RelayCommand]
        private async Task Volver() => await Shell.Current.GoToAsync("..");
    }
}
