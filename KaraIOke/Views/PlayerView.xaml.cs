using KaraIOke.ViewModels;

namespace KaraIOke.Views;

public partial class PlayerView : ContentPage
{
    public PlayerView(IServiceProvider serviceProvider)
    {
        InitializeComponent();

        BindingContext = serviceProvider.GetService<PlayerViewModel>();
    }
}

