using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BibliotekaPro.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditPage_new : ContentPage
    {
        Firebase firebase = new Firebase();
        public EditPage_new(User user)
        {
            InitializeComponent();
            entryEmail.Text = user.Email;
            entryName.Text = user.Name;
            entryId.Text = user.Id;
        }

        private async void buttonUpdate_Clicked(object sender, EventArgs e)
        {
            string name = entryName.Text;
            string email = entryEmail.Text;
            if (string.IsNullOrEmpty(name))
            {
                await DisplayAlert("Warning", "Please enter your name", "Cancel");
            }
            if (string.IsNullOrEmpty(email))
            {
                await DisplayAlert("Warning", "Please enter your email", "Cancel");
            }
            User user = new User();
            user.Id = entryId.Text;
            user.Name = entryName.Text;
            user.Email = entryEmail.Text;
            bool isUpdated = await firebase.Update(user);
            if(isUpdated)
            {
                await Navigation.PopModalAsync();
            }
            else
            {
                await DisplayAlert("Error", "Update failed", "Cancel");

            }

        }
    }
}