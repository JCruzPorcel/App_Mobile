using ITES_App.Models;
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
        private Button loginButton;
        private const string url_Logo = "Ites_Logo_Background.png";

        FirebaseHelper firebaseHelper;

        public LoginPage()
        {
            InitializeComponent();

            firebaseHelper = new FirebaseHelper();

            #region Initialization and Design

            NavigationPage.SetHasNavigationBar(this, false);

            var stackLayout = new StackLayout
            {
                BackgroundColor = Color.FromHex("#DCDAD5")
            };

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
                ReturnType = ReturnType.Next
            };

            dniEntry.Completed += (sender, e) =>
            {
                passwordEntry.Focus();
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
                IsPassword = true,
                PlaceholderColor = Color.Gray,
                TextColor = Color.Black,
                Keyboard = Keyboard.Default,
                ReturnType = ReturnType.Done
            };

            passwordEntry.Completed += (sender, e) =>
            {
                OnLoginButtonClicked(this, EventArgs.Empty);
            };

            loginButton = new Button
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
                HorizontalOptions = LayoutOptions.Start,
            };

            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += async (s, e) =>
            {
                await Navigation.PushAsync(new PasswordRecoveryPage());
            };

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

            loginButton.Clicked += OnLoginButtonClicked;

            #endregion


        }

        #region FIREBASE DATABASE REALTIME TEST

        private async void GuardarAlumnoButtonClicked(object sender, EventArgs e)
        {
            await firebaseHelper.AgregarAlumno("29032023", "admin", "Porcel.JuanCruz@example.com", "Perez Porcel Juan Cruz");
            await DisplayAlert("Éxito", "Alumno agregado correctamente", "Ok");
        }

        private async void CargarAlumnoButtonClicked(object sender, EventArgs e)
        {
            Alumno alumno = await firebaseHelper.ObtenerAlumno("41185616");

            if (alumno != null)
            {
                string mensaje = $"Nombre: {alumno.Nombre}\nEmail: {alumno.Email}\nPassword: {alumno.Password}";
                await DisplayAlert("Alumno encontrado", mensaje, "Ok");
            }
            else
            {
                await DisplayAlert("Error", "Alumno no encontrado", "Ok");
            }
        }

        #endregion


        private async void ForgotPasswordLabel_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PasswordRecoveryPage());
        }

        private async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            loginButton.IsEnabled = false;
            try
            {
                string dni = dniEntry.Text;
                string password = passwordEntry.Text;

                if (string.IsNullOrEmpty(dni) || string.IsNullOrEmpty(password))
                {
                    await DisplayAlert("Usuario no encontrado", "DNI o Clave incorrecta", "Ok");
                }
                else
                {
                    FirebaseHelper firebaseHelper = new FirebaseHelper();

                    Alumno alumno = await firebaseHelper.ObtenerAlumno(dni);

                    if (alumno == null || alumno.Password != password)
                    {
                        await DisplayAlert("Alumno no encontrado", "DNI o Clave incorrecta", "Ok");
                    }
                    else
                    {
                        await Navigation.PushAsync(new MainMenuPage());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                loginButton.IsEnabled = true;
            }
        }
    }
}