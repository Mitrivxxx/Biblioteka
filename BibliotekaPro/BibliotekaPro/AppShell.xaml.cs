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
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));

            //swiatlo
            _lightSensorService = DependencyService.Get<ILightSensorService>();
            _lightSensorService.LightSensorChanged += _lightSensorService_LightSensorChanged;
            _lightSensorService.StartListening();
        }

        private void _lightSensorService_LightSensorChanged(object sender, float e)
        {
            // Ustal, czy jest dzień czy noc na podstawie natężenia światła (lightLevel)
            if (e > 500) // Próg światła, który można dostosować w zależności od potrzeb
            {
                // Jest dzień
                //DisplayAlert("Dzień", "Wykryto jasne światło. Jest dzień.", "OK");
            }
            else
            {
                // Jest noc
                //DisplayAlert("Noc", "Niskie natężenie światła. Jest noc.", "OK");
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _lightSensorService.StopListening();
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
