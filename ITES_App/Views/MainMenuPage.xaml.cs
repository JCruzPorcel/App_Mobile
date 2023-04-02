using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ITES_App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainMenuPage : ContentPage
    {
        public MainMenuPage()
        {
            InitializeComponent();
        }

        private async void OnLogoutButtonClicked(object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}