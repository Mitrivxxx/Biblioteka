using Android.Media;
using BibliotekaPro.ViewModels;
using BibliotekaPro.Views;
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


        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
            Routing.RegisterRoute("appshell", typeof(AppShell));
            Routing.RegisterRoute("LoginPage", typeof(LoginPage));
            //swiatlo
            _lightSensorService = DependencyService.Get<ILightSensorService>();
            //_lightSensorService.LightSensorChanged += _lightSensorService_LightSensorChanged;
            _lightSensorService.StartListening();
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
            await Shell.Current.GoToAsync("//LoginPage");
        }

    }
}
