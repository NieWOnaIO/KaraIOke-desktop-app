using KaraIOke.ViewModels;

namespace KaraIOke.Views;

public partial class SearchView : ContentPage
{
	public SearchView(IServiceProvider serviceProvider)
	{
		InitializeComponent();

		BindingContext = serviceProvider.GetService<SearchViewModel>();
	}
}

