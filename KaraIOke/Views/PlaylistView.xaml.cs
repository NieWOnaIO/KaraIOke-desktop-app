using KaraIOke.ViewModels;

namespace KaraIOke.Views;

public partial class PlaylistView : ContentPage
{
    public PlaylistView(IServiceProvider serviceProvider)
    {
        InitializeComponent();

        BindingContext = serviceProvider.GetService<PlaylistViewModel>();
    }
}