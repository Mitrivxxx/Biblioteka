using System;
using System.Collections.Generic;
using System.Text;

namespace BibliotekaPro
{
    public class Book: IUserWithId
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Image { get; set; }
    }
}
