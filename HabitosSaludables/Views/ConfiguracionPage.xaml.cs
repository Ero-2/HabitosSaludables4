using HabitosSaludables.ViewModels;

namespace HabitosSaludables.Views;

public partial class ConfiguracionPage : ContentPage
{
    public ConfiguracionPage(ConfiguracionViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
