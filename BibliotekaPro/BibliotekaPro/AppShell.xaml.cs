using Android.Media;
using BibliotekaPro.ViewModels;
using BibliotekaPro.Views;
using Newtonsoft.Json;
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
            Routing.RegisterRoute("CreateAccount", typeof(CreateAccount));
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

                if (userNameLabel != null && userEmailLabel != null)
                {
                    userNameLabel.Text = $"Name: {user.Name}";
                    userEmailLabel.Text = $"Email: {user.Email}";
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

    }
}
