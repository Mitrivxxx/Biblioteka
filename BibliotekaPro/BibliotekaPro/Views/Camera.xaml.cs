using Android.Content;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Provider;
using Java.Util.Streams;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.Xaml;
using Color = Xamarin.Forms.Color;


namespace BibliotekaPro.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Camera : ContentPage
    {
        private readonly ILightSensorService _lightSensorService;
        Firebase firebase = new Firebase();
        public Camera()
        {
            InitializeComponent();

            _lightSensorService = DependencyService.Get<ILightSensorService>();
            _lightSensorService.LightSensorChanged += _lightSensorService_LightSensorChanged;
            _lightSensorService.StartListening();
        }
        public void _lightSensorService_LightSensorChanged(object sender, float e)
        {
            // Ustal, czy jest dzień czy noc na podstawie natężenia światła (lightLevel)
            if (e > 500) // Próg światła, który można dostosować w zależności od potrzeb
            {
                // Jest dzień
                backColor.BackgroundColor = Color.Gray;
                //DisplayAlert("dzien", e.ToString(), "OK");
                labelTest.Text = e.ToString();

            }
            else
            {
                // Jest noc
                labelTest.Text = e.ToString();

                backColor.BackgroundColor = Color.Black;


            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _lightSensorService.StopListening();
        }

        //zdjecie
       
        //wybiez zdj z galeri
        private async void button_Clicked_1(object sender, EventArgs e)
        {
            var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "Choise the product photo"
            });

            var stream = await result.OpenReadAsync();

            FotoProduto.Source = ImageSource.FromStream(() => stream);
        }

        private async void button_Clicked(object sender, EventArgs e)
        {
            try
            {
                // Sprawdź, czy urządzenie obsługuje kamerę
                if (!MediaPicker.IsCaptureSupported)
                {
                    await DisplayAlert("Błąd", "To urządzenie nie obsługuje kamery.", "OK");
                    return;
                }

                // Wywołaj MediaPicker, aby zrobić zdjęcie
                var photo = await MediaPicker.CapturePhotoAsync();

                if (photo == null)
                {
                    await DisplayAlert("Anulowano", "Nie wykonano zdjęcia.", "OK");
                    return;
                }

                // Zapisz zdjęcie na urządzeniu i pobierz jego lokalizację
                var savedFilePath = await SavePhotoAsync(photo);
                await DisplayAlert("Sukces", $"Zdjęcie zapisano w: {savedFilePath}", "OK");

                // Wyświetl zdjęcie w Image (jeśli masz kontrolkę Image na stronie)
                var stream = await photo.OpenReadAsync();
                FotoProduto.Source = ImageSource.FromStream(() => stream);
            }
            catch (PermissionException)
            {
                await DisplayAlert("Brak uprawnień", "Aplikacja nie ma uprawnień do korzystania z aparatu.", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Błąd", $"Coś poszło nie tak: {ex.Message}", "OK");
            }
        }
        //zdjecie zapisuje sie (niewiem gdzie)
/*        private async Task<string> SavePhotoAsync(FileResult photo)
        {
            // Pobierz ścieżkę do lokalnego katalogu na urządzeniu
            var localPath = System.IO.Path.Combine(FileSystem.AppDataDirectory, photo.FileName);

            using (var stream = await photo.OpenReadAsync())
            using (var newStream = File.OpenWrite(localPath))
                await stream.CopyToAsync(newStream);

            return localPath;
        }*/
        private async Task<string> SavePhotoAsync(FileResult photo)
        {
            try
            {
                // Pobierz ścieżkę do publicznego katalogu na urządzeniu (Galeria)
                var directory = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures).AbsolutePath;
                var localPath = System.IO.Path.Combine(directory, photo.FileName);

                using (var stream = await photo.OpenReadAsync())
                using (var newStream = File.OpenWrite(localPath))
                {
                    await stream.CopyToAsync(newStream);
                }

                // Powiadomienie systemu o nowym pliku w galerii
                MediaScannerConnection.ScanFile(Android.App.Application.Context, new string[] { localPath }, null, null);

                return localPath;  // Zwraca ścieżkę do zapisanego zdjęcia
            }
            catch (Exception ex)
            {
                // Obsługuje błędy
                Console.WriteLine("Error saving photo to gallery: " + ex.Message);
                return null;
            }
        }

        /*        public async Task<(string Base64Image, byte[] ImageBytes)> ConvertImageToBase64Async(Stream imageStream)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await imageStream.CopyToAsync(memoryStream);
                        var imageBytes = memoryStream.ToArray();
                        var base64Image = Convert.ToBase64String(imageBytes);
                        return (base64Image, imageBytes);
                    }
                }*/

        // zapis obrazka do bazy danych
        private async void Button_Clicked_2(object sender, EventArgs e)
        {
            /*try
            {
                // Wybór obrazu z galerii
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = "Select an image",
                    FileTypes = FilePickerFileType.Images
                });

                if (result != null)
                {
                    using (var stream = await result.OpenReadAsync())
                    {
                        // Kopiowanie strumienia do pamięci
                        using (var memoryStream = new MemoryStream())
                        {
                            await stream.CopyToAsync(memoryStream);

                            // Uzyskanie bajtów obrazu
                            var imageBytes = memoryStream.ToArray();

                            // Konwersja na Base64
                            var base64Image = Convert.ToBase64String(imageBytes);

                      // Tworzenie obiektu użytkownika z obrazem
                            var userWithImage = new User
                            {
                                Name = "Jane Smith",
                                Email = "janesmith@example.com",
                                Image = base64Image // Zapis Base64 do właściwości Image
                            };
 

                            // Zapis do Firebase
                            var isSavedWithImage = await firebase.Save(userWithImage);
                            if (isSavedWithImage)
                            {
                                await DisplayAlert("User saved successfully with an image!", "", "");
                            }
                            else
                            {
                                await DisplayAlert("Failed to save the user.", "", "");
                            }
                        }
                    }
                }
                else
                {
                    await DisplayAlert("No image selected.", "", "");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert($"Error: {ex.Message}", "", "");
            }*/
        }
    }
}