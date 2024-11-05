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
    public partial class AddPage_book : ContentPage
    {
        Firebase firebase = new Firebase();
        public AddPage_book()
        {
            InitializeComponent();
        }

        private async void buttonSave_Clicked(object sender, EventArgs e)
        {
            string name = entryName.Text;
            string author = entryAuthor.Text;
            if (string.IsNullOrEmpty(name))
            {
                await DisplayAlert("Warning", "Please enter your name", "Cancel");
            }
            if (string.IsNullOrEmpty(author))
            {
                await DisplayAlert("Warning", "Please enter your email", "Cancel");
            }

            Book book = new Book();
            book.Name = name;
            book.Author = author;


            var isSaved = await firebase.Save(book);

            if (isSaved)
            {
                await DisplayAlert("Information", "User has been saved", "Ok");
                Clear();
            }
            else
            {
                await DisplayAlert("Error", "User saved failed", "Ok");
                Clear();
            }
            Clear();

        }
        public void Clear()
        {
            entryName.Text = "";
            entryAuthor.Text = "";
        }
    }
}