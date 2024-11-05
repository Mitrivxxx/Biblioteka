using System;
using System.Collections.Generic;
using System.Text;

namespace BibliotekaPro
{
    public class User: IUserWithId
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Image {  get; set; }
    }
}
