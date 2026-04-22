using HabitosSaludables.Services;
using HabitosSaludables.ViewModels;

namespace HabitosSaludables.Views
{
    public partial class PerfilPage : ContentPage
    {
        private readonly PerfilViewModel _viewModel;

        // 👇 Constructor con inyección de dependencias
        public PerfilPage(PerfilViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = _viewModel = viewModel;
        }

        // 👇 Cargar datos cuando la página aparece
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.CargarDatosUsuarioAsync();
        }
    }
}