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
    public partial class InfoPage : ContentPage
    {
        public InfoPage(User user)
        {
            InitializeComponent();
            labelName.Text = user.Name;
            labelEmail.Text = user.Email;
        }
    }
}