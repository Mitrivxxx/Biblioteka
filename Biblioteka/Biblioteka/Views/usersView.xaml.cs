using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Biblioteka.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class usersView : ContentPage
    {
        public usersView()
        {
            InitializeComponent();
        }

        async private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new addPage());
        }
    }
}