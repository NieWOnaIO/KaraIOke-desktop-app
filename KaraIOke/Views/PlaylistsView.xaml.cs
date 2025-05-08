using KaraIOke.ViewModels;

namespace KaraIOke.Views;

public partial class PlaylistsView : ContentPage
{
    public PlaylistsView(IServiceProvider serviceProvider)
    {
        InitializeComponent();

        BindingContext = serviceProvider.GetService<PlaylistsViewModel>();
    }
}