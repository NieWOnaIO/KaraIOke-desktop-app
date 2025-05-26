using KaraIOke.ViewModels;

namespace KaraIOke.Views;

public partial class SearchView : ContentPage
{
    public SearchView(IServiceProvider serviceProvider)
    {
        InitializeComponent();

        BindingContext = serviceProvider.GetService<SearchViewModel>();
    }

    void OnEntryCompleted(object sender, EventArgs e)
    {
        ((SearchViewModel)BindingContext).SearchForSong.Execute(null);
    }
}

