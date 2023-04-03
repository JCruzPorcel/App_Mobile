﻿using ITES_App.Design;
using ITES_App.Models;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ITES_App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PasswordRecoveryPage : ContentPage
    {
        #region Variables 

        private const int MaxDigits = 8; // Caracter máximo DNI

        private static readonly ResourceDictionary _Resources = new ResourceDictionary
        {
            ["TitleText"] = "Recuperar Clave",
            ["SubtitleText"] = "Formulario de recuperación de clave",
            ["BackButtonText"] = "Volver",
            ["AcceptButtonText"] = "Aceptar",
            ["DniPlaceholder"] = "DNI",
            ["PasswordPlaceholder"] = "Clave Nueva",
            ["ConfirmPasswordPlaceholder"] = "Repetir Clave"
        };

        #endregion

        #region Reference's

        //private Entry emailEntry;
        private Entry dniEntry;
        private Entry passwordEntry;
        private Entry confirmPasswordEntry;
        private Button acceptButton;
        private Button backButton;
        private FirebaseHelper firebaseHelper;

        #endregion


        public PasswordRecoveryPage()
        {
            InitializeComponent();


            #region INITIALIZE COMPONENTS


            firebaseHelper = new FirebaseHelper();


            StackLayout stackLayout = new StackLayout { BackgroundColor = Color.FromHex(AppColors._Beige) };

            Frame frame1 = new Frame { BackgroundColor = Color.FromHex(AppColors._DarkGrey) };

            Label label1 = new Label
            {
                Text = _Resources["TitleText"].ToString(),
                TextColor = Color.White,
                FontSize = 20,
                FontAttributes = FontAttributes.Bold | FontAttributes.Italic,
                HorizontalTextAlignment = TextAlignment.Center
            };

            Image image1 = new Image { Source = AppColors._Logo };

            Label label2 = new Label
            {
                Text = _Resources["SubtitleText"].ToString(),
                TextColor = Color.FromHex(AppColors._DarkGrey),
                FontSize = 20,
                FontAttributes = FontAttributes.Bold | FontAttributes.Italic,
                HorizontalTextAlignment = TextAlignment.Center,
                Padding = new Thickness(10, 20, 10, 0)
            };

            backButton = new Button
            {
                Text = _Resources["BackButtonText"].ToString(),
                TextColor = Color.White,
                FontAttributes = FontAttributes.Bold | FontAttributes.Italic,
                CornerRadius = 20,
                Margin = new Thickness(0, 0, 10, 0),
                BackgroundColor = Color.FromHex(AppColors._Red),
            };

            acceptButton = new Button
            {
                Text = _Resources["AcceptButtonText"].ToString(),
                TextColor = Color.White,
                FontAttributes = FontAttributes.Bold,
                CornerRadius = 20,
                Margin = new Thickness(10, 0, 0, 0),
                BackgroundColor = Color.FromHex(AppColors._Blue),
            };

            Grid grid = new Grid
            {
                Margin = new Thickness(0, 20, 0, 10),
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

            /* emailEntry = new Entry()
             {
                 Placeholder = "Email",
                 PlaceholderColor = Color.Gray,
                 TextColor = Color.Black,
                 BackgroundColor = Color.WhiteSmoke,
                 Keyboard = Keyboard.Email,
                 ReturnType = ReturnType.Next
             };*/

            dniEntry = new Entry()
            {
                Placeholder = _Resources["DniPlaceholder"].ToString(),
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
                Placeholder = _Resources["PasswordPlaceholder"].ToString(),
                PlaceholderColor = Color.Gray,
                TextColor = Color.Black,
                BackgroundColor = Color.WhiteSmoke,
                ReturnType = ReturnType.Next,
                IsPassword = true
            };

            confirmPasswordEntry = new Entry()
            {
                Placeholder = _Resources["ConfirmPasswordPlaceholder"].ToString(),
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

            /* var emailFrame = new Frame()
             {
                 BackgroundColor = Color.WhiteSmoke,
                 CornerRadius = 10,
                 HasShadow = false,
                 Padding = new Thickness(5)
             };*/

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

            //stackLayout2.Children.Add(emailFrame);
            stackLayout2.Children.Add(dniFrame);
            stackLayout2.Children.Add(passwordFrame);
            stackLayout2.Children.Add(repeatPasswordFrame);
            stackLayout2.Children.Add(grid);

            //emailFrame.Content = emailEntry;
            dniFrame.Content = dniEntry;
            passwordFrame.Content = passwordEntry;
            repeatPasswordFrame.Content = confirmPasswordEntry;


            grid.Children.Add(backButton, 0, 0);
            grid.Children.Add(acceptButton, 1, 0);

            Content = stackLayout;
            frame1.Content = label1;
            #endregion

            #region Buttons
            backButton.Clicked += BackButton;
            acceptButton.Clicked += ConfirmRecoveryPassword;
            confirmPasswordEntry.Completed += ConfirmRecoveryPassword;
            #endregion
        }

        private async void ConfirmRecoveryPassword(object sender, EventArgs e)
        {
            acceptButton.IsEnabled = false;
            backButton.IsEnabled = false;
            try
            {
                string dni = dniEntry.Text;
                string newPassword = passwordEntry.Text;
                string confirmPassword = confirmPasswordEntry.Text;

                bool isFieldsFilled = !string.IsNullOrEmpty(dni) && !string.IsNullOrEmpty(newPassword) && !string.IsNullOrEmpty(confirmPassword);

                if (isFieldsFilled)
                {
                    Alumno alumno = await firebaseHelper.ObtenerAlumno(dni);

                    if (alumno != null)
                    {
                        if (newPassword == confirmPassword)
                        {
                            await DisplayAlert("Alumno encontrado", GetSuccessMessage(alumno), "Ok");

                            await firebaseHelper.ActualizarAlumno(alumno.DNI, newPassword, alumno.Email, alumno.Nombre);

                            dniEntry.Text = string.Empty;
                            passwordEntry.Text = string.Empty;
                            confirmPasswordEntry.Text = string.Empty;
                        }
                        else
                        {
                            await DisplayAlert("Error", "Las Claves no coinciden.\n\nIntentelo nuevamente.", "Ok");

                            passwordEntry.Text = string.Empty;
                            confirmPasswordEntry.Text = string.Empty;
                        }
                    }
                    else
                    {
                        await DisplayAlert("Error", "Alumno no encontrado", "Ok");
                    }
                }
                else
                {
                    await HandleEmptyFields(dni, newPassword, confirmPassword);
                }
            }
            finally
            {
                acceptButton.IsEnabled = true;
                backButton.IsEnabled = true;
            }
        }

        private async Task HandleEmptyFields(string dni, string newPassword, string confirmPassword)
        {
            if (string.IsNullOrEmpty(dni))
            {
                await DisplayAlert("Error", "Por favor, complete el campo de DNI.", "Ok");
            }
            else if (string.IsNullOrEmpty(newPassword))
            {
                await DisplayAlert("Error", "Por favor, complete el campo de Clave Nueva.", "Ok");
            }
            else if (string.IsNullOrEmpty(confirmPassword))
            {
                await DisplayAlert("Error", "Por favor, complete el campo de Repetir de Clave.", "Ok");
            }
        }

        private string GetSuccessMessage(Alumno alumno)
        {
            return $"DNI: {alumno.DNI}\nNombre: {alumno.Nombre}\nEmail: {alumno.Email}\n\nLa Clave se cambió exitosamente.";
        }

        private async void BackButton(object sender, System.EventArgs e)
        {
            acceptButton.IsEnabled = false;
            backButton.IsEnabled = false;

            try
            {
                await Navigation.PopAsync();
            }
            finally
            {
                acceptButton.IsEnabled = true;
                backButton.IsEnabled = true;
            }
        }
    }
}