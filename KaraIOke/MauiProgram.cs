using Microsoft.Extensions.Logging;
using KaraIOke.Services.AppEnvironment;
using KaraIOke.Services.Navigation;
using KaraIOke.Services.Search;
using KaraIOke.ViewModels;
using KaraIOke.Views;
using KaraIOke.Services.Playlists;

namespace KaraIOke;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("Inter.ttf", "Inter");
            })
            .RegisterAppServices()
            .RegisterViewModels()
            .RegisterViews();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }

    public static MauiAppBuilder RegisterAppServices(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddSingleton<NavigationService>();
        mauiAppBuilder.Services.AddSingleton<AppEnvironmentService>(
            serviceProvider =>
            {
                var aes = new AppEnvironmentService(new SearchMockService(), new SearchService(), new PlaylistMockService(), new PlaylistService());

                aes.updateDependencies(true); // hardcoded switching mocks for now (surely we will change it in the future :))

                return aes;
            }
        );

        return mauiAppBuilder;
    }
    public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddSingleton<MainViewModel>();
        mauiAppBuilder.Services.AddSingleton<SearchViewModel>();
        mauiAppBuilder.Services.AddSingleton<PlayerViewModel>();
        mauiAppBuilder.Services.AddSingleton<PlaylistDetailsViewModel>();
        mauiAppBuilder.Services.AddSingleton<PlaylistsViewModel>();
        mauiAppBuilder.Services.AddSingleton<AddingViewModel>();

        return mauiAppBuilder;
    }

    public static MauiAppBuilder RegisterViews(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddSingleton<MainView>();
        mauiAppBuilder.Services.AddTransient<SearchView>();
        mauiAppBuilder.Services.AddTransient<PlayerView>();
        mauiAppBuilder.Services.AddTransient<PlaylistDetailsView>();
        mauiAppBuilder.Services.AddTransient<PlaylistsView>();
        mauiAppBuilder.Services.AddTransient<AddingView>();

        return mauiAppBuilder;
    }
}
