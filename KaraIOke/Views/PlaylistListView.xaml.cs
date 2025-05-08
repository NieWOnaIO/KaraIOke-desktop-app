using KaraIOke.ViewModels;

namespace KaraIOke.Views;

public partial class PlaylistListView : ContentPage
{
    public PlaylistListView(IServiceProvider serviceProvider)
    {
        InitializeComponent();

        BindingContext = serviceProvider.GetService<PlaylistListViewModel>();
    }
}