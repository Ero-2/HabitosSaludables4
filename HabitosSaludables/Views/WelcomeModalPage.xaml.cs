namespace HabitosSaludables.Views;

public partial class WelcomeModalPage : ContentPage
{
    public WelcomeModalPage() => InitializeComponent();

    private async void OnCloseClicked(object sender, EventArgs e) => await Navigation.PopModalAsync();

    // Maneja taps en el fondo (backdrop) para cerrar el modal
    private async void OnBackdropTapped(object sender, EventArgs e)
    {
        // Si se desea, aquí se puede filtrar el origen del tap para evitar cierres no deseados.
        await Navigation.PopModalAsync();
    }
}