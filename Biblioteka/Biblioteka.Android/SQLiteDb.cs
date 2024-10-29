using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Biblioteka.Droid;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLiteDb))]

namespace Biblioteka.Droid
{
    public class SQLiteDb : ISQLiteDb
    {
        public SQLiteConnection GetConnection()
        {
            var dbName = "mydatabase.db3"; // Nazwa pliku bazy danych
            var folderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); // Ścieżka do folderu aplikacji
            var fullPath = Path.Combine(folderPath, dbName); // Pełna ścieżka do pliku bazy danych
            return new SQLiteConnection(fullPath); // Zwrócenie połączenia
        }
    }
}