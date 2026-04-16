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

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        await AnimateButton((Button)sender);
        await _viewModel.RegistrarCommand.ExecuteAsync(null);
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        await AnimateButton((Button)sender);
        await this.TranslateTo(-1000, 0, 300, Easing.CubicOut);
        _viewModel.IrALoginCommand.Execute(null);
        await this.TranslateTo(0, 0, 0);
    }

    private async Task AnimateButton(Button button)
    {
        await button.ScaleTo(1.1, 100);
        await button.ScaleTo(1, 100);
    }
}