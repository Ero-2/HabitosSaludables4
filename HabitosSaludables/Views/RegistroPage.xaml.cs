using HabitosSaludables.ViewModels;

namespace HabitosSaludables.Views;

public partial class RegistroPage : ContentPage
{
    private readonly RegistroViewModel _viewModel;

    public RegistroPage(RegistroViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    // 🔙 MÉTODO PARA EL BOTÓN DE REGRESO (FUNCIONAL)
    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        // Animación sutil del botón
        if (sender is Border border)
        {
            await border.ScaleTo(0.9, 50);
            await border.ScaleTo(1, 50);
        }

        // Navegación hacia atrás usando Shell
        await Shell.Current.GoToAsync("..");
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        await AnimateButton((Button)sender);
        await _viewModel.RegistrarCommand.ExecuteAsync(null);
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        if (sender is Label label)
        {
            await label.FadeTo(0.5, 100);
            await label.FadeTo(1, 100);
        }

        await this.TranslateTo(-1000, 0, 300, Easing.CubicOut);
        _viewModel.IrALoginCommand.Execute(null);
        await this.TranslateTo(1000, 0, 0);
        await this.TranslateTo(0, 0, 200);
    }

    private async Task AnimateButton(Button button)
    {
        await button.ScaleTo(0.95, 50);
        await button.ScaleTo(1, 50);
    }
}