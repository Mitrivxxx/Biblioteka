﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BibliotekaPro.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddPage : ContentPage
	{
		Firebase firebase = new Firebase();
		public AddPage ()
		{
			InitializeComponent ();
		}

        private async void buttonSave_Clicked(object sender, EventArgs e)
        {
			string name = entryName.Text;
			string email = entryEmail.Text;
            string login = entryName.Text;
            string password = entryEmail.Text;
            if (string.IsNullOrEmpty(name))
			{
				await DisplayAlert("Warning", "Please enter your name", "Cancel");
			}
            if (string.IsNullOrEmpty(email))
            {
                await DisplayAlert("Warning", "Please enter your email", "Cancel");
            }
            if (string.IsNullOrEmpty(login))
            {
                await DisplayAlert("Warning", "Please enter your login", "Cancel");
            }
            if (string.IsNullOrEmpty(password))
            {
                await DisplayAlert("Warning", "Please enter your password", "Cancel");
            }

            User user = new User();
			user.Name = name;
			user.Email = email;
            user.Login = login;
            user.Password = password;


            var isSaved = await firebase.Save(user);
        
			if(isSaved)
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
			entryEmail.Text = "";
            entryLogin.Text = "";
            entryPassword.Text = "";
        }
    }
}