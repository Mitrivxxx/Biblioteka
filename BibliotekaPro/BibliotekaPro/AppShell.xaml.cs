using Android.Media;
using BibliotekaPro.ViewModels;
using BibliotekaPro.Views;
using Newtonsoft.Json;
using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;
using System;
using System.Collections.Generic;
using Xamarin.Forms;


namespace BibliotekaPro
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        private readonly ILightSensorService _lightSensorService;
        private LoginPage _loginPage;
        private UsersPage _usersPage;
        //cos
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("LoginUserInformation", typeof(LoginUserInformation));
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
            Routing.RegisterRoute("appshell", typeof(AppShell));
            Routing.RegisterRoute("LoginPage", typeof(LoginPage));
            //swiatlo
            _lightSensorService = DependencyService.Get<ILightSensorService>();
            //_lightSensorService.LightSensorChanged += _lightSensorService_LightSensorChanged;
            _lightSensorService.StartListening();

            if (Application.Current.Properties.ContainsKey("user"))
            {
                var userJson = Application.Current.Properties["user"] as string;
                var user = JsonConvert.DeserializeObject<User>(userJson);

                // Ustawiamy dane użytkownika w Label
                var userNameLabel = this.FindByName<Label>("UserNameLabel");
                var userEmailLabel = this.FindByName<Label>("UserEmailLabel");
                var userLoginLabel = this.FindByName<Label>("UserLoginLabel");


                if (userNameLabel != null && userEmailLabel != null)
                {
                    userNameLabel.Text = user.Name;
                    userEmailLabel.Text = $"Email: {user.Email}";
                    userLoginLabel.Text = $"Login: {user.Login}";

                }

                if (user.Name != "admin" && user.Email != "admin" && user.Password != "admin")
                {
                    usersPageAdmin.IsVisible = false;
                    booksPageAdmin.IsVisible = false;
                    BooksPageForUser.IsVisible = true;
                }
                else
                {
                    usersPageAdmin.IsVisible = true;
                    booksPageAdmin.IsVisible = true;
                    BooksPageForUser.IsVisible = false;

                }
            }
            else
            {
                // Obsługuje przypadek, gdy brak danych użytkownika
                Console.WriteLine("User not found in Application Properties.");
            }
        }
        /*       public void _lightSensorService_LightSensorChanged(object sender, float e)
               {
                   // Ustal, czy jest dzień czy noc na podstawie natężenia światła (lightLevel)
                   if (e > 500) // Próg światła, który można dostosować w zależności od potrzeb
                   {
                       // Jest dzień
                       //DisplayAlert("dzien", e.ToString(), "OK");

                   }
                   else
                   {
                       // Jest noc
                       //DisplayAlert("Noc", e.ToString(), "OK");

                   }
               }

               protected override void OnDisappearing()
               {
                   base.OnDisappearing();
                   _lightSensorService.StopListening();
               }*/

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            if (Application.Current.Properties.ContainsKey("user"))
            {
                Application.Current.Properties.Remove("user");
                await Application.Current.SavePropertiesAsync();  // Zapisanie zmian
            }
            await Shell.Current.GoToAsync("//LoginPage");
        }




        private async void MenuItem_Clicked_1(object sender, EventArgs e)
        {
            var result = await CrossFingerprint.Current.AuthenticateAsync(new AuthenticationRequestConfiguration(
            "Autoryzacja biometryczna",
            "Użyj odcisku palca, aby przejść dalej"));

            if (result.Authenticated)
            {
                // Przejdź do nowej strony
                await Navigation.PushAsync(new BooksPage());
            }
            else
            {
                await DisplayAlert("Błąd", "Autoryzacja nie powiodła się", "OK");
            }
        }

    }
}
