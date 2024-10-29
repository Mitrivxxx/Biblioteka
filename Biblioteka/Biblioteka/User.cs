using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Biblioteka
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public int BirthYear { get; set; }
        public string AdressEmail { get; set; }
    }
}
