using ITES_App.Design;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ITES_App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainMenuPage : ContentPage
    {
        private Button listOfNotes_Button;
        private Button certificates_Button;
        private Button inscriptions_Button;
        private Button classroom_Button;
        private Button assists_Button;

        private static readonly ResourceDictionary _Resources = new ResourceDictionary
        {
            ["TitleText"] = "ITES - Autogestión",
            ["LogoutButtonText"] = "Cerrar Sesión",
            ["ListOfNotesButtonText"] = "Listado de Notas",
            ["CertificatesButtonText"] = "Certificados",
            ["InscriptionsButtonText"] = "Inscripciones",
            ["ClassroomButtonText"] = "Classroom",
            ["AssitsButtonText"] = "Asistencias",
        };

        public MainMenuPage()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, true);
            Title = _Resources["LogoutButtonText"].ToString();
            NavigationPage.SetIconColor(this, Color.White);

            StackLayout stackLayout = new StackLayout { BackgroundColor = Color.FromHex(AppColors._Beige) };

            StackLayout stackLayout1 = new StackLayout { BackgroundColor = Color.FromHex(AppColors._DarkGrey) };

            Label textTitle = new Label
            {
                Text = _Resources["TitleText"].ToString(),
                TextColor = Color.White,
                FontSize = 20,
                FontAttributes = FontAttributes.Bold | FontAttributes.Italic,
                TextDecorations = TextDecorations.None,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
            };

            var imageLogo = new Image { Source = AppColors._Logo };

            var frameButtons = new Frame
            {
                BackgroundColor = Color.WhiteSmoke,
                Padding = 10,
                CornerRadius = 20,
                Margin = new Thickness(30, 60, 30, 20),
            };

            var stackLayoutMenuButtons = new StackLayout { Spacing = 12, };


            listOfNotes_Button = new Button
            {
                Text = _Resources["ListOfNotesButtonText"].ToString(),
                TextColor = Color.WhiteSmoke,
                FontAttributes = FontAttributes.Bold,
                BackgroundColor = Color.FromHex(AppColors._LightBlue),
                CornerRadius = 50,
                Margin = new Thickness(30, 10, 30, 0)
            };

            certificates_Button = new Button
            {
                Text = _Resources["CertificatesButtonText"].ToString(),
                TextColor = Color.WhiteSmoke,
                FontAttributes = FontAttributes.Bold,
                BackgroundColor = Color.FromHex(AppColors._LightBlue),
                CornerRadius = 50,
                Margin = new Thickness(30, 0, 30, 0)
            };

            inscriptions_Button = new Button
            {
                Text = _Resources["InscriptionsButtonText"].ToString(),
                TextColor = Color.WhiteSmoke,
                FontAttributes = FontAttributes.Bold,
                BackgroundColor = Color.FromHex(AppColors._LightBlue),
                CornerRadius = 50,
                Margin = new Thickness(30, 0, 30, 0)
            };

            classroom_Button = new Button
            {
                Text = _Resources["ClassroomButtonText"].ToString(),
                TextColor = Color.WhiteSmoke,
                FontAttributes = FontAttributes.Bold,
                BackgroundColor = Color.FromHex(AppColors._LightGreen),
                CornerRadius = 50,
                Margin = new Thickness(30, 0, 30, 0)
            };

            assists_Button = new Button
            {
                Text = _Resources["AssitsButtonText"].ToString(),
                TextColor = Color.WhiteSmoke,
                FontAttributes = FontAttributes.Bold,
                BackgroundColor = Color.FromHex(AppColors._LightRed),
                CornerRadius = 50,
                Margin = new Thickness(30, 0, 30, 10)
            };


            Content = stackLayout;

            stackLayout.Children.Add(stackLayout1);
            stackLayout1.Children.Add(textTitle);
            stackLayout1.Children.Add(imageLogo);


            stackLayoutMenuButtons.Children.Add(listOfNotes_Button);
            stackLayoutMenuButtons.Children.Add(certificates_Button);
            stackLayoutMenuButtons.Children.Add(inscriptions_Button);
            stackLayoutMenuButtons.Children.Add(classroom_Button);
            stackLayoutMenuButtons.Children.Add(assists_Button);



            stackLayout.Children.Add(frameButtons);

            frameButtons.Content = stackLayoutMenuButtons;


            listOfNotes_Button.Clicked += OnListOfNotesButtonClicked;
            certificates_Button.Clicked += OnCertificateButtonClicked;
            inscriptions_Button.Clicked += OnInscriptionsButtonClicked;
            classroom_Button.Clicked += OnClassroomButtonClicked;
            assists_Button.Clicked += OnAssistsButtonClicked;

        }



        private async void OnListOfNotesButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ListOfNotesPage());
        }

        private async void OnCertificateButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CertificatesPage());
        }

        private async void OnInscriptionsButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new InscriptionPage());
        }

        private async void OnClassroomButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ClassroomPage());
        }

        private async void OnAssistsButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AssistsPage());
        }
    }
}