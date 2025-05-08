using KaraIOke.ViewModels;

namespace KaraIOke.Views;

public partial class PlaylistDetailsView : ContentPage
{
    public PlaylistDetailsView(IServiceProvider serviceProvider)
    {
        InitializeComponent();

        BindingContext = serviceProvider.GetService<PlaylistDetailsViewModel>();
    }
}