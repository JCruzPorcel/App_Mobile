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
        private const string url_Logo = "Ites_logo.png";

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
                Margin = new Thickness(0, 10, 0, 0),
            };

            forgotPasswordLabel.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() =>
                {
                    // ToDo: código para manejar el evento Tapped de la etiqueta.
                    //DisplayAlert("Recuperar Clave", "Ingrese su correo", "Cerrar", "Ok");
                    //CargarAlumnoButtonClicked(this, EventArgs.Empty);
                     GuardarAlumnoButtonClicked(this, EventArgs.Empty);

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
            //GuardarAlumnoButtonClicked;

            #endregion


        }

        #region FIREBASE DATABASE REALTIME TEST
        
        private async void GuardarAlumnoButtonClicked(object sender, EventArgs e)
        {
            // Guardar el objeto Alumno en Firebase Realtime Database
            await firebaseHelper.AgregarAlumno("41185616", "admin123", "Porcel.JuanCruz.ar@gmail.com", "Juan Cruz Perez Porcel");
            await DisplayAlert("Éxito", "Alumno agregado correctamente", "Ok");
        }

        private async void CargarAlumnoButtonClicked(object sender, EventArgs e)
        {
            // Obtener el objeto Alumno correspondiente al DNI "12345678A" desde Firebase Realtime Database
            Alumno alumno = await firebaseHelper.ObtenerAlumno("41185616");

            if (alumno != null)
            {
                // Si el objeto Alumno existe en Firebase Realtime Database, mostrar sus valores en un mensaje
                string mensaje = $"Nombre: {alumno.Nombre}\nEmail: {alumno.Email}\nPassword: {alumno.Password}";
                await DisplayAlert("Alumno encontrado", mensaje, "Ok");
            }
            else
            {
                // Si el objeto Alumno no existe en Firebase Realtime Database, mostrar un mensaje de error
                await DisplayAlert("Error", "Alumno no encontrado", "Ok");
            }
        }
        
        #endregion

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
