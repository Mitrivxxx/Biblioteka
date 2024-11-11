using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.Xaml;


namespace BibliotekaPro.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Camera : ContentPage
    {
        public Camera()
        {
            InitializeComponent();
        }
        private async void TakePhotoButton_Clicked(object sender, EventArgs e)
        {
            
        }

    }
}