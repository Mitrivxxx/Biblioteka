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
    public partial class BooksPageForUser : ContentPage
    {
        public BooksPageForUser()
        {
            InitializeComponent();
        }
        Firebase firebase = new Firebase();

        protected override async void OnAppearing()
        {
            var book = await firebase.GetAll<Book>();
            BookListView.ItemsSource = null;
            BookListView.ItemsSource = book;
            BookListView.IsRefreshing = false;
        }


        private void searchInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            //zaraz zrobie
        }

        private void BookListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
            {
                return;
            }
            var book = e.Item as Book;
            Navigation.PushModalAsync(new InfoPage(book));
            ((ListView)sender).SelectedItem = null;
        }

        private void tapEdit_Tapped(object sender, EventArgs e)
        {

        }

        private void tapBin_Tapped(object sender, EventArgs e)
        {

        }
    }
}
