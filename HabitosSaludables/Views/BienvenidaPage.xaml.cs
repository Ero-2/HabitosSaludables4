using HabitosSaludables.ViewModels;

namespace HabitosSaludables.Views;

public partial class BienvenidaPage : ContentPage
{
    private readonly BienvenidaViewModel _viewModel;

    public BienvenidaPage(BienvenidaViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        await AnimateButton((Button)sender);
        _viewModel.GoToRegistroCommand.Execute(null);
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        await AnimateButton((Button)sender);
        _viewModel.GoToLoginCommand.Execute(null);
    }

    private async Task AnimateButton(Button button)
    {
        await button.ScaleTo(1.1, 100);
        await button.ScaleTo(1, 100);
    }
}