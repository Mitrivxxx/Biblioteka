using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BibliotekaPro
{
    public interface ICameraService
    {
        Task<string> TakePhotoAsync();
        void OnPhotoTaken(string path);
    }
}
