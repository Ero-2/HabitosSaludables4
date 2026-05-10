using HabitosSaludables.ViewModels;

namespace HabitosSaludables.Views;

public partial class RegistroActividadPage : ContentPage
{
    private readonly RegistroActividadViewModel _viewModel;

    public RegistroActividadPage(RegistroActividadViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.CargarAsync();
    }
}
