using BibliotekaPro.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace BibliotekaPro.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}