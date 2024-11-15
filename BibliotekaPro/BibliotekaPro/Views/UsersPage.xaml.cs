using Android.Media.TV;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BibliotekaPro.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class UsersPage : ContentPage
	{
		Firebase firebase = new Firebase();
		public UsersPage ()
		{
			InitializeComponent ();
		}
		protected override async void OnAppearing()
		{
			var user = await firebase.GetAll<User>();
			UserListView.ItemsSource = null;
			UserListView.ItemsSource = user;
			UserListView.IsRefreshing = false;
		}
        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
			Navigation.PushModalAsync(new AddPage());
        }

        private void UserListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
			if(e.Item==null)
			{
				return;
			}
			var user = e.Item as User;
			Navigation.PushModalAsync(new InfoPage(user));
			((ListView)sender).SelectedItem = null;
        }

		//edit icon
        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
			//DisplayAlert("Edit", "Do you want to edit?", "Ok");

			string id = ((TappedEventArgs)e).Parameter.ToString();
			var user = await firebase.GetById(id);
			if (user == null)
			{
				await DisplayAlert("Warning","Data not found.", "Ok");
			}
			user.Id = id;
			await Navigation.PushModalAsync(new EditPage_new(user));
		
		}
		//delete icon
        private async void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            var response = await DisplayAlert("Delete", "Do you want to delete?","Yes" ,"No");
			if(response)
			{
                string id = ((TappedEventArgs)e).Parameter.ToString();
                bool isDeleted = await firebase.Delete(id);
				{
					if (isDeleted)
					{
                        await DisplayAlert("Information", "User has been deleted", "Ok");
						OnAppearing();
                    }
					else
					{
                        await DisplayAlert("Error", "Deleted failed", "Ok");
                    }
                }
			}
        }


        private async void searchInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchValue = searchInput.Text;
            if (!String.IsNullOrEmpty(searchValue))
            {
                var user = await firebase.SearchByName(searchValue);
                UserListView.ItemsSource = null;
                UserListView.ItemsSource = user;
            }
            else
            {
                await DisplayAlert("Error", "onapperaning", "Ok");

                OnAppearing();
            }
        }





    }
}