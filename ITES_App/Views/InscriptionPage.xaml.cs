using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ITES_App.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class InscriptionPage : ContentPage
	{
		public InscriptionPage ()
		{
			InitializeComponent ();

            listView.ItemSelected += OnItemSelected;
        }

        private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            label.Text = e.SelectedItem.ToString();
            listView.IsVisible = false;
        }

        private void OnLabelTapped(object sender, EventArgs e)
        {
            listView.IsVisible = !listView.IsVisible;
        }

    }
}