using HabitosSaludables.Services;
using HabitosSaludables.ViewModels;
using HabitosSaludables.Views;
using Microsoft.Extensions.Logging;

namespace HabitosSaludables;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // 🔹 Servicio de base de datos (con ruta)
        string dbPath = Path.Combine(FileSystem.AppDataDirectory, "habitos.db3");
        builder.Services.AddSingleton(new DatabaseService(dbPath));

        // ViewModels
        builder.Services.AddTransient<BienvenidaViewModel>();
        builder.Services.AddTransient<RegistroViewModel>();
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<HomeViewModel>();
        builder.Services.AddTransient<MisHabitosViewModel>();
        builder.Services.AddTransient<AgregarHabitoViewModel>();
        builder.Services.AddTransient<PerfilViewModel>();
        builder.Services.AddTransient<EstadisticasViewModel>();
        builder.Services.AddTransient<ConfiguracionViewModel>();
        builder.Services.AddTransient<RegistroActividadViewModel>();
        builder.Services.AddTransient<EditarPerfilViewModel>();

        // Pages
        builder.Services.AddTransient<BienvenidaPage>();
        builder.Services.AddTransient<RegistroPage>();
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<HomePage>();
        builder.Services.AddTransient<WelcomeModalPage>();
        builder.Services.AddTransient<MisHabitosPage>();
        builder.Services.AddTransient<AgregarHabitoPage>();
        builder.Services.AddTransient<PerfilPage>();
        builder.Services.AddTransient<EstadisticasPage>();
        builder.Services.AddTransient<ConfiguracionPage>();
        builder.Services.AddTransient<RegistroActividadPage>();
        builder.Services.AddTransient<EditarPerfilPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}