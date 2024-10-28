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
    public partial class addPage : ContentPage
    {
        public addPage()
        {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            string firstName = FirstNameEntry.Text;
            string lastName = LastNameEntry.Text;
            string birthYear = BirthYearEntry.Text;
            string street = StreetEntry.Text;

        }
    }
}