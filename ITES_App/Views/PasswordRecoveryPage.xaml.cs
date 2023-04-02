using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ITES_App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PasswordRecoveryPage : ContentPage
    {

        private const int MaxDigits = 8; // Caracter maximo DNI

        private Entry emailEntry;
        private Entry dniEntry;
        private Entry passwordEntry;
        private Entry repeatPasswordEntry;
        private Button acceptBtn;
        private Button backBtn;
        private const string url_Logo = "Ites_Logo_Background.png";

        public PasswordRecoveryPage()
        {
            InitializeComponent();


            #region INITIALIZE COMPONENTS
            StackLayout stackLayout = new StackLayout { BackgroundColor = Color.FromHex("#DCDAD5") };

            Frame frame1 = new Frame { BackgroundColor = Color.FromHex("#343A40") };

            Label label1 = new Label
            {
                Text = "Recuperar Clave",
                TextColor = Color.White,
                FontSize = 20,
                FontAttributes = FontAttributes.Bold | FontAttributes.Italic,
                HorizontalTextAlignment = TextAlignment.Center
            };

            Image image1 = new Image { Source = url_Logo };

            Label label2 = new Label
            {
                Text = "Formulario de recuperación de clave",
                TextColor = Color.FromHex("#343A40"),
                FontSize = 20,
                FontAttributes = FontAttributes.Bold | FontAttributes.Italic,
                HorizontalTextAlignment = TextAlignment.Center,
                Padding = new Thickness(10, 20, 10, 0)
            };

            backBtn = new Button
            {
                Text = "Volver",
                TextColor = Color.White,
                FontAttributes = FontAttributes.Bold | FontAttributes.Italic,
                CornerRadius = 20,
                Margin = new Thickness(0, 0, 10, 0),
                BackgroundColor = Color.FromHex("#BF6171"),
            };

            acceptBtn = new Button
            {
                Text = "Aceptar",
                TextColor = Color.White,
                FontAttributes = FontAttributes.Bold,
                CornerRadius = 20,
                Margin = new Thickness(10, 0, 0, 0),
                BackgroundColor = Color.FromHex("#5F7FA6"),
            };

            Grid grid = new Grid
            {
                Margin = new Thickness(0,20,0,10),
                ColumnDefinitions =
            {
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
            }
            };

            StackLayout stackLayout2 = new StackLayout()
            {
                Spacing = 10
            };

            emailEntry = new Entry()
            {
                Placeholder = "Email",
                PlaceholderColor = Color.Gray,
                TextColor = Color.Black,
                BackgroundColor = Color.WhiteSmoke,
                Keyboard = Keyboard.Email,
                ReturnType = ReturnType.Next
            };

            dniEntry = new Entry()
            {
                Placeholder = "DNI",
                PlaceholderColor = Color.Gray,
                TextColor = Color.Black,
                BackgroundColor = Color.WhiteSmoke,
                ReturnType = ReturnType.Next,
                Keyboard = Keyboard.Numeric
            };

            dniEntry.TextChanged += (sender, e) =>
            {
                if (e.NewTextValue.Length > MaxDigits)
                {
                    dniEntry.Text = e.OldTextValue;
                }
            };

            passwordEntry = new Entry()
            {
                Placeholder = "Clave Nueva",
                PlaceholderColor = Color.Gray,
                TextColor = Color.Black,
                BackgroundColor = Color.WhiteSmoke,
                ReturnType = ReturnType.Next,
                IsPassword = true
            };

            repeatPasswordEntry = new Entry()
            {
                Placeholder = "Repetir Clave",
                PlaceholderColor = Color.Gray,
                TextColor = Color.Black,
                BackgroundColor = Color.WhiteSmoke,
                ReturnType = ReturnType.Done,
                IsPassword = true
            };

            Frame frame2 = new Frame
            {
                CornerRadius = 15,
                HasShadow = true,
                Margin = new Thickness(20),
            };

            var emailFrame = new Frame()
            {
                BackgroundColor = Color.WhiteSmoke,
                CornerRadius = 10,
                HasShadow = false,
                Padding = new Thickness(5)
            };

            var dniFrame = new Frame()
            {
                BackgroundColor = Color.WhiteSmoke,
                CornerRadius = 10,
                HasShadow = false,
                Padding = new Thickness(5)
            };

            var passwordFrame = new Frame()
            {
                BackgroundColor = Color.WhiteSmoke,
                CornerRadius = 10,
                HasShadow = false,
                Padding = new Thickness(5)
            };

            var repeatPasswordFrame = new Frame()
            {
                BackgroundColor = Color.WhiteSmoke,
                CornerRadius = 10,
                HasShadow = false,
                Padding = new Thickness(5)
            };


            stackLayout.Children.Add(frame1);
            stackLayout.Children.Add(image1);
            stackLayout.Children.Add(label2);
            stackLayout.Children.Add(frame2);

            frame2.Content = stackLayout2;

            stackLayout2.Children.Add(emailFrame);
            stackLayout2.Children.Add(dniFrame);
            stackLayout2.Children.Add(passwordFrame);
            stackLayout2.Children.Add(repeatPasswordFrame);
            stackLayout2.Children.Add(grid);

            emailFrame.Content = emailEntry;
            dniFrame.Content = dniEntry;
            passwordFrame.Content = passwordEntry;
            repeatPasswordFrame.Content = repeatPasswordEntry;


            grid.Children.Add(backBtn, 0, 0);
            grid.Children.Add(acceptBtn, 1, 0);

            Content = stackLayout;
            frame1.Content = label1;
#endregion

            #region Buttons
            backBtn.Clicked += BackButton;
            acceptBtn.Clicked += ConfirmRecoveryPassword;
            #endregion
        }

        private async void ConfirmRecoveryPassword(object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void BackButton(object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}