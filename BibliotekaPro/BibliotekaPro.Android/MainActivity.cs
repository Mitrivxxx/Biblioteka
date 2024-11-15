using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Xamarin.Forms;
using Android.Content;
using System.Linq;
using Xamarin.Essentials;
using Plugin.Permissions;

namespace BibliotekaPro.Droid
{
    [Activity(Label = "BibliotekaPro", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        //Zdjecia
        internal static MainActivity Instance { get; private set; }
        public const int TakePhotoId = 1000;
        //---
        protected override void OnCreate(Bundle savedInstanceState)
        {
            Instance = this;
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());

        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            if(Xamarin.Essentials.DeviceInfo.Version.Major >= 13 && (permissions.Where(p => p.Equals("android.permission.WRITE_EXTERNAL_STORAGE")).Any() || permissions.Where(p => p.Equals("android.permission.READ_EXTERNAL_STORAGE")).Any()))
                {
                var wIdx = Array.IndexOf(permissions, "android.permission.WRITE_EXTERNAL_STORAGE");
                var rIdx = Array.IndexOf(permissions, "android.permission.READ_EXTERNAL_STORAGE");

                if (wIdx != -1 && wIdx < permissions.Length) grantResults[wIdx] = Permission.Granted;
                if (rIdx != -1 && rIdx < permissions.Length) grantResults[rIdx] = Permission.Granted;
            }

            Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    
        //zdjecia
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == TakePhotoId && resultCode == Result.Ok)
            {
                string photoPath = ((CameraService)DependencyService.Get<ICameraService>()).CreateImageFile().AbsolutePath;
                DependencyService.Get<ICameraService>().OnPhotoTaken(photoPath);
            }
        }
    }
}