using HabitosSaludables.ViewModels;

namespace HabitosSaludables.Views;

public partial class HomePage : ContentPage
{
    private readonly HomeViewModel _viewModel;

    public HomePage(HomeViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    private void OnRegistrarActividadClicked(object sender, EventArgs e)
    {
        _viewModel.RegistrarActividadCommand?.Execute(null);
    }

    private void OnVerEstadisticasClicked(object sender, EventArgs e)
    {
        _viewModel.VerEstadisticasCommand?.Execute(null);
    }

    private void OnConfiguracionClicked(object sender, EventArgs e)
    {
        _viewModel.ConfiguracionCommand?.Execute(null);
    }

    private async void OnPerfilClicked(object sender, EventArgs e)
    {
        // CORREGIDO: Se quitan las "//" porque PerfilPage no es una pestaña principal (ShellContent)
        await Shell.Current.GoToAsync("//PerfilPage");
    }

    private async void OnMisHabitosClicked(object sender, EventArgs e)
    {
        // CORREGIDO: Se navega relativamente (apilando la página)
        await Shell.Current.GoToAsync(nameof(MisHabitosPage));
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        // Ejecutamos la lógica de cerrado de sesión del ViewModel si existe
        if (_viewModel.CerrarSesionCommand != null && _viewModel.CerrarSesionCommand.CanExecute(null))
        {
            _viewModel.CerrarSesionCommand.Execute(null);
        }

        // Redirigimos al Login. Aquí SÍ se usa "//" porque LoginPage es un ShellContent de raíz en AppShell.xaml
        await Shell.Current.GoToAsync("//LoginPage");
    }
}