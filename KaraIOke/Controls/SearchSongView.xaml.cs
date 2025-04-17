namespace KaraIOke.Controls
{
    public partial class SearchSongView : ContentView
    {
        public static readonly BindableProperty SongNameProperty = BindableProperty.Create(nameof(SongName), typeof(string), typeof(SearchSongView), string.Empty);

        public string SongName
        {
            get => (string)GetValue(SearchSongView.SongNameProperty);
            set => SetValue(SearchSongView.SongNameProperty, value);
        }

        public SearchSongView() {
            InitializeComponent();
        }
    }
}