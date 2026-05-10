using HabitosSaludables.Views;

namespace HabitosSaludables;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Solo registrar rutas que NO están definidas como ShellContent en AppShell.xaml
        Routing.RegisterRoute(nameof(MisHabitosPage), typeof(MisHabitosPage));
        Routing.RegisterRoute(nameof(AgregarHabitoPage), typeof(AgregarHabitoPage));
        Routing.RegisterRoute(nameof(EstadisticasPage), typeof(EstadisticasPage));
        Routing.RegisterRoute(nameof(ConfiguracionPage), typeof(ConfiguracionPage));
        Routing.RegisterRoute(nameof(RegistroActividadPage), typeof(RegistroActividadPage));
        Routing.RegisterRoute(nameof(WelcomeModalPage), typeof(WelcomeModalPage));
        Routing.RegisterRoute(nameof(EditarPerfilPage), typeof(EditarPerfilPage));
    }
}