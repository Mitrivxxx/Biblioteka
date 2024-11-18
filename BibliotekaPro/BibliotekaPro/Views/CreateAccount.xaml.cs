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
    public partial class CreateAccount : ContentPage
    {
        public CreateAccount()
        {
            InitializeComponent();
        }
        Firebase firebase = new Firebase();
        private async void Button_Clicked(object sender, EventArgs e)
        {
            string name = entryName.Text;
            string name_2 = entryName_2.Text;
            string email = entryEmail.Text;
            string login = entryName.Text;
            string password = entryPassword.Text;
            string password_2 = entryPassword_2.Text;
            bool flag = true;
            if (string.IsNullOrEmpty(name))
            {
                await DisplayAlert("Warning", "Please enter your name", "Cancel");
                flag = false;
            }
            if (string.IsNullOrEmpty(name_2))
            {
                await DisplayAlert("Warning", "Please enter your surname", "Cancel");
                flag = false;
            }
            if (string.IsNullOrEmpty(email))
            {
                await DisplayAlert("Warning", "Please enter your email", "Cancel");
                flag = false;
            }
            if (string.IsNullOrEmpty(login))
            {
                await DisplayAlert("Warning", "Please enter your login", "Cancel");
                flag = false;
            }
            if (string.IsNullOrEmpty(password))
            {
                await DisplayAlert("Warning", "Please enter your password", "Cancel");
                flag = false;
            }
            if (string.IsNullOrEmpty(password_2))
            {
                await DisplayAlert("Warning", "Please enter your password", "Cancel");
                flag = false;
            }

            if (flag==true)
            {
                string fulname = name + " " + name_2;

                User user = new User();
                user.Name = fulname;
                user.Email = email;
                user.Login = login;
                user.Password = password;
                var isSaved = await firebase.Save(user);
                if (isSaved)
                {
                    await DisplayAlert("Information", "user created successfully", "Ok");
                    Application.Current.MainPage = new LoginPage();
                }
                Clear();
            }
            


        }

            
        public void Clear()
        {
            entryName.Text = "";
            entryEmail.Text = "";
            entryLogin.Text = "";
            entryPassword.Text = "";
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            Application.Current.MainPage = new LoginPage();
        }
    }
}