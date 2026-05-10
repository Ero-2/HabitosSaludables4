using HabitosSaludables.ViewModels;

namespace HabitosSaludables.Views;

public partial class EditarPerfilPage : ContentPage
{
    public EditarPerfilPage(EditarPerfilViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
