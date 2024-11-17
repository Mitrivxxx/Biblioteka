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
using SkiaSharp;


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
            string base64Image = null;

            try
            {
                Console.WriteLine("Starting image selection...");

                // Wybór obrazu z galerii
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = "Select an image",
                    FileTypes = FilePickerFileType.Images
                });

                Console.WriteLine(result != null
                    ? $"File selected: {result.FileName}"
                    : "No file selected.");

                if (result == null)
                {
                    // Ustawienie domyślnego obrazu, gdy użytkownik nie wybierze pliku
                    base64Image = GetDefaultBase64Image();
                    Console.WriteLine("Default image set.");
                }
                else
                {
                    // Otwieranie strumienia pliku
                    using (var originalStream = await result.OpenReadAsync())
                    {
                        if (originalStream == null)
                        {
                            throw new Exception("Could not open file stream.");
                        }

                        Console.WriteLine("File stream opened successfully.");

                        // Przeskalowanie obrazu do 20x20 pikseli
                        var resizedImageBytes = ResizeImageTo20x20(originalStream);

                        Console.WriteLine("Image resized successfully.");

                        // Konwersja przeskalowanego obrazu do Base64
                        base64Image = Convert.ToBase64String(resizedImageBytes);

                        Console.WriteLine("Image converted to Base64 successfully.");
                    }
                }

                // Tworzenie obiektu użytkownika z obrazem
                var userWithImage = new User
                {
                    Name = "Jane Smith",
                    Email = "janesmith@example.com",
                    Image = base64Image // Zapis Base64 do właściwości Image
                };

                Console.WriteLine("User object created successfully.");

                // Zapis do Firebase
                var isSavedWithImage = await firebase.Save(userWithImage);

                if (isSavedWithImage)
                {
                    await DisplayAlert("User saved successfully with an image!", "", "");
                    Console.WriteLine("User saved successfully.");
                }
                else
                {
                    await DisplayAlert("Failed to save the user.", "", "");
                    Console.WriteLine("Failed to save the user.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception caught: {ex.Message}");
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");

                // Zapewnienie, że nawet w przypadku błędu będzie domyślne zdjęcie
                if (string.IsNullOrEmpty(base64Image))
                {
                    base64Image = GetDefaultBase64Image();
                    Console.WriteLine("Default image set due to error.");
                }
            }

        }
        private string GetDefaultBase64Image()
        {
            // Możesz tutaj dodać Base64 dla swojego domyślnego obrazu
            return "puste";
        }
        // Funkcja do przeskalowania obraz
        private byte[] ResizeImageTo20x20(System.IO.Stream originalStream)
        {
            using (var inputStream = new SKManagedStream(originalStream))
            {
                var originalBitmap = SKBitmap.Decode(inputStream);

                // Tworzenie nowego bitmapu o rozdzielczości 20x20
                var resizedBitmap = originalBitmap.Resize(new SKImageInfo(20, 20), SKFilterQuality.High);

                if (resizedBitmap == null)
                {
                    throw new Exception("Failed to resize the image.");
                }

                using (var image = SKImage.FromBitmap(resizedBitmap))
                using (var data = image.Encode(SKEncodedImageFormat.Png, 100)) // PNG o wysokiej jakości
                {
                    return data.ToArray(); // Zwrócenie bajtów przeskalowanego obrazu
                }
            }
        }
    }
}