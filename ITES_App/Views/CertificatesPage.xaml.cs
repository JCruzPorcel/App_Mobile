using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Application = Xamarin.Forms.Application;

namespace ITES_App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CertificatesPage : ContentPage
    {
        public CertificatesPage()
        {
            InitializeComponent();

        }

        private void OnPickerFocused(object sender, FocusEventArgs e)
        {
            Picker picker = (Picker)sender;
            if (Application.Current.RequestedTheme == OSAppTheme.Light)
            {
                picker.TitleColor = Color.Black;
            }
            else
            {
                picker.TitleColor = Color.White;
            }
        }

        private void OnPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            Picker picker = (Picker)sender;
            picker.TextColor = Color.Black;
        }

    }
}