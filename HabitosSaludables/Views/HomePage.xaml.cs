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

    private async void OnRegistrarActividadClicked(object sender, EventArgs e)
    {
        await AnimateButton((Button)sender);
        _viewModel.RegistrarActividadCommand.Execute(null);
    }

    private async void OnVerEstadisticasClicked(object sender, EventArgs e)
    {
        await AnimateButton((Button)sender);
        _viewModel.VerEstadisticasCommand.Execute(null);
    }

    private async void OnConfiguracionClicked(object sender, EventArgs e)
    {
        await AnimateButton((Button)sender);
        _viewModel.ConfiguracionCommand.Execute(null);
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        await AnimateButton((Button)sender);
        _viewModel.CerrarSesionCommand.Execute(null);
    }

    private async Task AnimateButton(Button button)
    {
        await button.ScaleTo(1.1, 100);
        await button.ScaleTo(1, 100);
    }
}