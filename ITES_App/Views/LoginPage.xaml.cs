using ITES_App.Views;
using System;
using Xamarin.Forms;

namespace ITES_App
{
    public partial class LoginPage : ContentPage
    {
        private const int MaxDigits = 8; // Caracter maximo DNI

        private Entry dniEntry;
        private Entry passwordEntry;
        private const string url_Logo = "Ites_logo.png";

        public LoginPage()
        {
            InitializeComponent();

            #region Initialization and Design

            NavigationPage.SetHasNavigationBar(this, false);

            var stackLayout = new StackLayout
            {
                BackgroundColor = Color.FromHex("#DCDAD5")
            };

            /* var label = new Label
             {
                 Text = "ITES Mobile",
                 FontAttributes = FontAttributes.Bold,
                 FontSize = 40,
                 TextColor = Color.FromHex("#194780"),
                 Margin = new Thickness(0, 20, 0, 0),
                 HorizontalOptions = LayoutOptions.Center
             };*/

            var image = new Image
            {
                Source = url_Logo,
                HeightRequest = 200,
                WidthRequest = 200,
                Margin = new Thickness(20, 100, 20, 70)
            };

            var frame = new Frame
            {
                Margin = new Thickness(20, 20, 20, 20),
                CornerRadius = 15,
                HasShadow = true,
                VerticalOptions = LayoutOptions.Center
            };

            var innerStackLayout = new StackLayout
            {
                Spacing = 10
            };

            var dniFrame = new Frame
            {
                BackgroundColor = Color.WhiteSmoke,
                CornerRadius = 10,
                HasShadow = false,
                Padding = new Thickness(5)
            };

            dniEntry = new Entry
            {
                Placeholder = "DNI",
                PlaceholderColor = Color.Gray,
                TextColor = Color.Black,
                Keyboard = Keyboard.Numeric,
            };

            dniEntry.TextChanged += (sender, e) =>
            {
                if (e.NewTextValue.Length > MaxDigits)
                {
                    dniEntry.Text = e.OldTextValue;
                }

                // Separar DNI por "punto"
                /*   else
                   {
                       var digitsOnly = new string(e.NewTextValue.Where(char.IsDigit).ToArray());
                       var formattedText = "";

                       for (int i = 0; i < digitsOnly.Length; i++)
                       {
                           if (i == 2 || i == 5)
                           {
                               formattedText += ".";
                           }

                           formattedText += digitsOnly[i];
                       }

                       dniEntry.Text = formattedText;
                   }*/
            };

            var passwordFrame = new Frame
            {
                BackgroundColor = Color.WhiteSmoke,
                CornerRadius = 10,
                HasShadow = false,
                Padding = new Thickness(5)
            };

            passwordEntry = new Entry
            {
                Placeholder = "Clave",
                PlaceholderColor = Color.Gray,
                TextColor = Color.Black,
                Keyboard = Keyboard.Default,
                IsPassword = true
            };

            var loginButton = new Button
            {
                Text = "Iniciar Sesión",
                TextColor = Color.White,
                TextTransform = TextTransform.None,
                BackgroundColor = Color.FromHex("#42D885"),
                FontAttributes = FontAttributes.Bold,
                CornerRadius = 50
            };

            var forgotPasswordLabel = new Label
            {
                Text = "Olvidé mi clave",
                FontAttributes = FontAttributes.Italic,
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                TextColor = Color.FromHex("#8DB5FF"),
                Margin = new Thickness(0, 10, 0, 0),
            };

            forgotPasswordLabel.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() =>
                {
                    // ToDo: código para manejar el evento Tapped de la etiqueta.
                    DisplayAlert("Recuperar Clave", "Ingrese su correo", "Ok", "Cerrar");
                })
            });

            dniFrame.Content = dniEntry;
            passwordFrame.Content = passwordEntry;
            innerStackLayout.Children.Add(dniFrame);
            innerStackLayout.Children.Add(passwordFrame);
            innerStackLayout.Children.Add(loginButton);
            innerStackLayout.Children.Add(forgotPasswordLabel);
            frame.Content = innerStackLayout;
            //stackLayout.Children.Add(label);
            stackLayout.Children.Add(image);
            stackLayout.Children.Add(frame);
            Content = stackLayout;

            #endregion

            #region Buttons

            loginButton.Clicked += OnLoginButtonClicked;

            #endregion

        }

        private async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(passwordEntry.Text) || string.IsNullOrEmpty(dniEntry.Text))
            {
                await DisplayAlert("Usuario no encontrado", "DNI o Clave incorrecta", "Ok");
            }
            else
            {
                await Navigation.PushAsync(new MainMenuPage());
            }
        }
    }
}
