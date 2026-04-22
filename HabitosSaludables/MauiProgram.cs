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

        // 🔹 ViewModels (incluyendo los nuevos)
        builder.Services.AddTransient<BienvenidaViewModel>();
        builder.Services.AddTransient<RegistroViewModel>();
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<HomeViewModel>();
        builder.Services.AddTransient<MisHabitosViewModel>();      // 👈 Nuevo
        builder.Services.AddTransient<AgregarHabitoViewModel>();
        builder.Services.AddTransient<PerfilViewModel>();// 👈 Nuevo

        // 🔹 Páginas (incluyendo las nuevas)
        builder.Services.AddTransient<BienvenidaPage>();
        builder.Services.AddTransient<RegistroPage>();
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<HomePage>();
        builder.Services.AddTransient<WelcomeModalPage>();
        builder.Services.AddTransient<MisHabitosPage>();           // 👈 Nuevo
        builder.Services.AddTransient<AgregarHabitoPage>();        // 👈 Nuevo

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}