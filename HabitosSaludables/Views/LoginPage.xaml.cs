using HabitosSaludables.ViewModels;

namespace HabitosSaludables.Views;

public partial class LoginPage : ContentPage
{
    private readonly LoginViewModel _viewModel;

    public LoginPage(LoginViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;

        _viewModel.LoginExitoso += OnLoginExitoso;
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        await AnimateButton((Button)sender);
        _viewModel.IniciarSesionCommand.Execute(null);
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        await AnimateButton((Button)sender);
        await this.TranslateTo(-1000, 0, 300, Easing.CubicOut);
        _viewModel.IrARegistroCommand.Execute(null);
        await this.TranslateTo(0, 0, 0);
    }

    private async Task OnLoginExitoso()
    {
        await Navigation.PushModalAsync(new WelcomeModalPage());
    }

    private async Task AnimateButton(Button button)
    {
        await button.ScaleTo(1.1, 100);
        await button.ScaleTo(1, 100);
    }
}