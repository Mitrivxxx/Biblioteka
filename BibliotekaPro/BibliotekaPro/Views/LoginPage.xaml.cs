using Newtonsoft.Json;
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
    public partial class LoginPage : ContentPage
    {
        Firebase firebase = new Firebase();

        public LoginPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var login = entryLogin.Text;  // Załóżmy, że masz Entry o nazwie LoginEntry
            var password = entryPassword.Text;  // Załóżmy, że masz Entry o nazwie PasswordEntry

            // Twój obiekt Firebase

            // Sprawdzamy, czy login i hasło są poprawne
            var user = await firebase.LoginAsync(login, password);

            if (user != null)
            {
                // Po udanym logowaniu, zapisujemy dane użytkownika w Properties
                Application.Current.Properties["user"] = JsonConvert.SerializeObject(user);  // Zapisujemy dane użytkownika
                await Application.Current.SavePropertiesAsync();  // Zapisujemy zmiany w Properties

                // Zmiana strony głównej na AppShell
                Application.Current.MainPage = new AppShell();

                // Pokazujemy komunikat o udanym logowaniu
                await DisplayAlert("Success", "Logged in successfully", "OK");
            }
            else
            {
                await DisplayAlert("Error", "Invalid username or password", "OK");
            }
        }

    }
}