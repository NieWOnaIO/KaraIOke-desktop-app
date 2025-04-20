using KaraIOke.ViewModels;

namespace KaraIOke.Views;

public partial class MainView : ContentPage
{
	public MainView(IServiceProvider serviceProvider)
	{
		InitializeComponent();

		BindingContext = serviceProvider.GetService<MainViewModel>();
	}
}

