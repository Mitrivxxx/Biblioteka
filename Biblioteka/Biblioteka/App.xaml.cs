using Biblioteka.Services;
using Biblioteka.Views;
using SQLite;
using System;
using System.Data.Common;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Biblioteka
{
    public partial class App : Application
    {
        public static SQLiteConnection DbConnection { get; private set; }
        public App()
        {
            InitializeComponent();

            var db = DependencyService.Get<ISQLiteDb>().GetConnection();
            db.CreateTable<User>();

            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
