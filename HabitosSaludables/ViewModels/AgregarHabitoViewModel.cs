using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HabitosSaludables.Models;
using HabitosSaludables.Services;

namespace HabitosSaludables.ViewModels
{
    public partial class AgregarHabitoViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;

        [ObservableProperty] private string _nombre = string.Empty;
        [ObservableProperty] private string _categoria = "Salud";
        [ObservableProperty] private string _icono = "🌿";

        // Cámara
        [ObservableProperty] private bool _usarCamara = false;
        [ObservableProperty] private bool _fotoTomada = false;
        [ObservableProperty] private ImageSource? _fotoPreview;

        // Ubicación
        [ObservableProperty] private bool _rastrearUbicacion = false;
        [ObservableProperty] private string _latitudTexto = string.Empty;
        [ObservableProperty] private string _longitudTexto = string.Empty;
        [ObservableProperty] private bool _ubicacionObtenida = false;

        private double? _latitud;
        private double? _longitud;
        private string? _fotoPath;

        public List<string> Categorias { get; } = new()
        {
            "Salud", "Deporte", "Alimentación", "Mental", "Otros"
        };

        public AgregarHabitoViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        partial void OnUsarCamaraChanged(bool value)
        {
            if (value)
                _ = SolicitarPermisoCamaraAsync();
            else
            {
                FotoTomada = false;
                FotoPreview = null;
                _fotoPath = null;
            }
        }

        partial void OnRastrearUbicacionChanged(bool value)
        {
            if (value)
                _ = SolicitarPermisoUbicacionAsync();
            else
            {
                LatitudTexto = string.Empty;
                LongitudTexto = string.Empty;
                UbicacionObtenida = false;
                _latitud = null;
                _longitud = null;
            }
        }

        private async Task SolicitarPermisoCamaraAsync()
        {
            var status = await Permissions.RequestAsync<Permissions.Camera>();
            if (status != PermissionStatus.Granted)
            {
                UsarCamara = false;
                await Shell.Current.DisplayAlert(
                    "Permiso requerido",
                    "Activa el acceso a la cámara en Configuración del dispositivo.",
                    "OK");
            }
        }

        private async Task SolicitarPermisoUbicacionAsync()
        {
            var status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            if (status != PermissionStatus.Granted)
            {
                RastrearUbicacion = false;
                await Shell.Current.DisplayAlert(
                    "Permiso requerido",
                    "Activa el acceso a la ubicación en Configuración del dispositivo.",
                    "OK");
                return;
            }
            await ObtenerUbicacionActualAsync();
        }

        [RelayCommand]
        private async Task TomarFoto()
        {
            try
            {
                if (!MediaPicker.Default.IsCaptureSupported)
                {
                    await Shell.Current.DisplayAlert("No disponible", "Este dispositivo no soporta captura de fotos.", "OK");
                    return;
                }

                var foto = await MediaPicker.Default.CapturePhotoAsync();
                if (foto == null) return;

                // Guardar foto en directorio de la app con nombre único
                var carpeta = FileSystem.AppDataDirectory;
                var nombreArchivo = $"habito_{DateTime.Now:yyyyMMdd_HHmmss}.jpg";
                var rutaDestino = Path.Combine(carpeta, nombreArchivo);

                using var streamOrigen = await foto.OpenReadAsync();
                using var streamDestino = File.OpenWrite(rutaDestino);
                await streamOrigen.CopyToAsync(streamDestino);

                _fotoPath = rutaDestino;
                FotoPreview = ImageSource.FromFile(rutaDestino);
                FotoTomada = true;
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"No se pudo tomar la foto: {ex.Message}", "OK");
            }
        }

        [RelayCommand]
        private void EliminarFoto()
        {
            if (_fotoPath != null && File.Exists(_fotoPath))
                File.Delete(_fotoPath);

            _fotoPath = null;
            FotoPreview = null;
            FotoTomada = false;
        }

        [RelayCommand]
        private async Task ObtenerUbicacionActualAsync()
        {
            try
            {
                LatitudTexto = "Obteniendo...";
                LongitudTexto = string.Empty;

                var location = await Geolocation.GetLocationAsync(new GeolocationRequest
                {
                    DesiredAccuracy = GeolocationAccuracy.Medium,
                    Timeout = TimeSpan.FromSeconds(10)
                });

                if (location != null)
                {
                    _latitud = location.Latitude;
                    _longitud = location.Longitude;
                    LatitudTexto = $"{location.Latitude:F6}";
                    LongitudTexto = $"{location.Longitude:F6}";
                    UbicacionObtenida = true;
                }
                else
                {
                    LatitudTexto = "No disponible";
                    LongitudTexto = string.Empty;
                    UbicacionObtenida = false;
                }
            }
            catch
            {
                LatitudTexto = "Error";
                LongitudTexto = string.Empty;
                UbicacionObtenida = false;
            }
        }

        [RelayCommand]
        private async Task GuardarHabito()
        {
            if (string.IsNullOrWhiteSpace(Nombre))
            {
                await Shell.Current.DisplayAlert("Error", "El nombre es obligatorio", "OK");
                return;
            }

            var nuevo = new Habito
            {
                Nombre = Nombre,
                Categoria = Categoria,
                Icono = Icono,
                FechaCreacion = DateTime.Now,
                UsarCamara = UsarCamara,
                FotoPath = _fotoPath,
                Latitud = _latitud,
                Longitud = _longitud
            };

            await _databaseService.SaveHabitoAsync(nuevo);
            await Shell.Current.DisplayAlert("¡Éxito!", "Hábito guardado correctamente", "OK");
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        private async Task Cancelar() => await Shell.Current.GoToAsync("..");
    }
}
