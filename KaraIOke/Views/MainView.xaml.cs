using System.Diagnostics;
using KaraIOke.ViewModels;

namespace KaraIOke.Views;

public partial class MainView : ContentPage
{
    public MainView(IServiceProvider serviceProvider)
    {
        InitializeComponent();

        BindingContext = serviceProvider.GetService<MainViewModel>();
    }

    void OnEntryCompleted(object sender, EventArgs e)
    {
        ((MainViewModel)BindingContext).SearchForSong.Execute(null);
    }
}
