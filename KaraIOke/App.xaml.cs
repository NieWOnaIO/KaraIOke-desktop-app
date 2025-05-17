using KaraIOke.Views;

namespace KaraIOke;

public partial class App : Application
{
    MainView _mainView;
    public App(IServiceProvider serviceProvider)
    {
        InitializeComponent();

        _mainView = serviceProvider.GetService<MainView>() ?? throw new InvalidOperationException("MainView is not registered.");
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new NavigationPage(_mainView));
    }
}