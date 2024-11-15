using Android.App;
using Android.Content;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using BibliotekaPro.Droid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(CameraService))] // Zarejestruj implementację CameraService dla DependencyService

namespace BibliotekaPro.Droid
{
    public class CameraService:ICameraService
    {
        private TaskCompletionSource<string> _taskCompletionSource;
        private readonly Activity _activity;

        public CameraService()
        {
            _activity = MainActivity.Instance;
        }

        public Task<string> TakePhotoAsync()
        {
            _taskCompletionSource = new TaskCompletionSource<string>();

            Intent intent = new Intent(MediaStore.ActionImageCapture);
            if (intent.ResolveActivity(_activity.PackageManager) != null)
            {
                Java.IO.File photoFile = CreateImageFile();
                if (photoFile != null)
                {
                    var photoUri = AndroidX.Core.Content.FileProvider.GetUriForFile(_activity,
                        $"{_activity.PackageName}.fileprovider", photoFile);
                    intent.PutExtra(MediaStore.ExtraOutput, photoUri);
                    _activity.StartActivityForResult(intent, MainActivity.TakePhotoId);
                }
            }
            return _taskCompletionSource.Task;
        }

        public Java.IO.File CreateImageFile()
        {
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string storageDir = _activity.GetExternalFilesDir(Android.OS.Environment.DirectoryPictures).AbsolutePath;
            Java.IO.File image = new Java.IO.File(storageDir, $"JPEG_{timestamp}.jpg");
            return image;
        }

        public void OnPhotoTaken(string path)
        {
            _taskCompletionSource?.SetResult(path);
        }
    }
}