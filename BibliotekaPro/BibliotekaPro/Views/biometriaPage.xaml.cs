using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;
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
    public partial class biometriaPage : ContentPage
    {
        public biometriaPage()
        {

            InitializeComponent();
            bio();
            
        }
        private async void bio()
        {
            await DisplayAlert("Authorization", "Soon, you’ll be asked for your fingerprint", "Ok");
            var result = await CrossFingerprint.Current.AuthenticateAsync(new AuthenticationRequestConfiguration(
            "Autoryzacja biometryczna",
            "Użyj odcisku palca, aby przejść dalej"));

            if (result.Authenticated)
            {
                await Navigation.PushAsync(new UsersPage());
            }
            else
            {
                await Navigation.PushAsync(new AppShell());
            }
        }
    }
}