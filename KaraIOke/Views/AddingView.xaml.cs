using KaraIOke.ViewModels;

namespace KaraIOke.Views;

public partial class AddingView : ContentPage
{
    public AddingView(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        
        BindingContext = serviceProvider.GetService<AddingViewModel>();
    }
}