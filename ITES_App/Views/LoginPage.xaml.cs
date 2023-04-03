using ITES_App.Design;
using ITES_App.Models;
using ITES_App.Views;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ITES_App
{
    public partial class LoginPage : ContentPage
    {
        #region Variables
        private const int MaxDigits = 8; // Caracter máximo DNI
        private const string placeHolder_DNI = "DNI";
        private const string placeHolder_Password = "Clave";
        private const string login_Text = "Iniciar Sesión";
        private const string forgotPassword_Text = "Olvidé mi clave";

        #endregion

        #region Reference's
        // Design
        private Entry dniEntry;
        private Entry passwordEntry;
        private Button loginButton;
        private Label forgotPasswordLabel;
        private TapGestureRecognizer tapGestureRecognizer;

        FirebaseHelper firebaseHelper;
        #endregion


        public LoginPage()
        {
            InitializeComponent();


            firebaseHelper = new FirebaseHelper();


            #region Design

            NavigationPage.SetHasNavigationBar(this, false);

            var stackLayout = new StackLayout
            {
                BackgroundColor = Color.FromHex(AppColors._Beige)
            };

            var image = new Image
            {
                Source = AppColors._Logo,
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
                Placeholder = placeHolder_DNI,
                PlaceholderColor = Color.Gray,
                TextColor = Color.Black,
                Keyboard = Keyboard.Numeric,
                ReturnType = ReturnType.Next
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
                Placeholder = placeHolder_Password,
                IsPassword = true,
                PlaceholderColor = Color.Gray,
                TextColor = Color.Black,
                Keyboard = Keyboard.Default,
                ReturnType = ReturnType.Done
            };

            loginButton = new Button
            {
                Text = login_Text,
                TextColor = Color.White,
                TextTransform = TextTransform.None,
                BackgroundColor = Color.FromHex(AppColors._Green),
                FontAttributes = FontAttributes.Bold,
                CornerRadius = 50
            };

            forgotPasswordLabel = new Label
            {
                Text = forgotPassword_Text,
                FontAttributes = FontAttributes.Italic,
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                TextColor = Color.FromHex(AppColors._DarkBlue),
                HorizontalOptions = LayoutOptions.Start,
            };

            tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += ForgotPasswordLabel_Tapped;

            forgotPasswordLabel.GestureRecognizers.Add(tapGestureRecognizer);

            var labelContainer = new StackLayout
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Padding = 0,
                Children = { forgotPasswordLabel }
            };



            dniFrame.Content = dniEntry;
            passwordFrame.Content = passwordEntry;
            innerStackLayout.Children.Add(dniFrame);
            innerStackLayout.Children.Add(passwordFrame);
            innerStackLayout.Children.Add(loginButton);
            innerStackLayout.Children.Add(forgotPasswordLabel);
            frame.Content = innerStackLayout;
            stackLayout.Children.Add(image);
            stackLayout.Children.Add(frame);
            Content = stackLayout;

            #endregion

            #region Buttons

            passwordEntry.Completed += (sender, e) =>
            {
                OnLoginButtonClicked(this, EventArgs.Empty);
            };

            loginButton.Clicked += OnLoginButtonClicked;

            #endregion

        }

        #region Method's

        private async void ForgotPasswordLabel_Tapped(object sender, EventArgs e)
        {
            loginButton.IsEnabled = false;
            forgotPasswordLabel.GestureRecognizers.Remove(tapGestureRecognizer);

            try
            {
                passwordEntry.Text = string.Empty;
                dniEntry.Text = string.Empty;
            }
            finally
            {
                forgotPasswordLabel.GestureRecognizers.Add(tapGestureRecognizer);
                loginButton.IsEnabled = true;
                await Navigation.PushAsync(new PasswordRecoveryPage());

            }
        }

        private async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            loginButton.IsEnabled = false;
            forgotPasswordLabel.GestureRecognizers.Remove(tapGestureRecognizer);

            try
            {
                string dni = dniEntry.Text;
                string password = passwordEntry.Text;

                if (string.IsNullOrEmpty(dni) || string.IsNullOrEmpty(password))
                {
                    await HandleEmptyFields(dni, password);
                }
                else
                {
                    var usuario = await firebaseHelper.ObtenerAlumno(dni);

                    if (usuario == null)
                    {
                        await DisplayAlert("DNI Inválido", "El usuario no se encuentra registrado", "Aceptar");
                    }
                    else if (usuario.Password != password)
                    {
                        await DisplayAlert("Clave Inválida", "La Clave es incorrecta", "Aceptar");
                    }
                    else
                    {
                        try
                        {
                            passwordEntry.Text = string.Empty;
                            dniEntry.Text = string.Empty;
                        }
                        finally
                        {
                            await Navigation.PushAsync(new MainMenuPage());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                forgotPasswordLabel.GestureRecognizers.Add(tapGestureRecognizer);
                loginButton.IsEnabled = true;
            }
        }

        private async Task HandleEmptyFields(string dni, string password)
        {
            if (string.IsNullOrEmpty(dni) && string.IsNullOrEmpty(password))
            {
                await DisplayAlert("Error", "Por favor, complete el campo de DNI y Clave.", "Ok");
            }
            else if (string.IsNullOrEmpty(dni))
            {
                await DisplayAlert("Error", "Por favor, complete el campo de DNI.", "Ok");
            }
            else if (string.IsNullOrEmpty(password))
            {
                await DisplayAlert("Error", "Por favor, complete el campo de Clave.", "Ok");
            }
        }

        #endregion
    }

}