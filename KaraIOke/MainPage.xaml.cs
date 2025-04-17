namespace KaraIOke;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

    private async void OnSearchBtnClicked(object sender, EventArgs e)
    {
		await Navigation.PushModalAsync(new SearchPage());
    }
}

