namespace KaraIOke;

public partial class SearchPage : ContentPage
{
	public SearchPage()
	{
		InitializeComponent();
	}

    private async void OnBackBtnClicked(object sender, EventArgs e)
    {
		await Navigation.PopModalAsync();
    }
}

