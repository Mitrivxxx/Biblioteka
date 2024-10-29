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
            LoadUsers();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            var user = new User
            {
                FirstName = FirstNameEntry.Text,
                LastName = LastNameEntry.Text,
                BirthYear = int.Parse(BirthYearEntry.Text),
                AdressEmail = AdressEmailEntry.Text
            };

            var db = DependencyService.Get<ISQLiteDb>().GetConnection();
            db.Insert(user);

            LoadUsers();
        }

        private void LoadUsers()
        {
            var db = DependencyService.Get<ISQLiteDb>().GetConnection();
            var users = db.Table<User>().ToList();
            UsersListView.ItemsSource = users;
        }
    }
}