using Biblioteka.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace Biblioteka.Views
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