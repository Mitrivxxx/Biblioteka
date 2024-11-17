using BibliotekaPro.Models;
using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BibliotekaPro
{
    public class Firebase
    {
        FirebaseClient firebaseClient = new FirebaseClient("https://bibliotekapro-eeddd-default-rtdb.firebaseio.com/");

        //add 
/*        public async Task<bool> Save<T>(T obj)
        {
            var collectionName = typeof(T).Name;
            var data = await firebaseClient.Child(collectionName).PostAsync(JsonConvert.SerializeObject(obj));

            return !string.IsNullOrEmpty(data.Key);
        }*/

        public async Task<bool> Save<T>(T obj, string base64Image = null)
        {
            // Jeśli obiekt ma właściwość "Image", ustaw ją
            if (!string.IsNullOrEmpty(base64Image))
            {
                var imageProperty = typeof(T).GetProperty("Image");
                if (imageProperty != null && imageProperty.PropertyType == typeof(string))
                {
                    imageProperty.SetValue(obj, base64Image);
                }
            }

            var collectionName = typeof(T).Name;
            var data = await firebaseClient.Child(collectionName).PostAsync(JsonConvert.SerializeObject(obj));

            return !string.IsNullOrEmpty(data.Key);
        }

        //save /get all
        public async Task<List<T>> GetAll<T>() where T : class, new()
        {
            try
            {
                var items = await firebaseClient.Child(typeof(T).Name).OnceAsync<T>();

                return items.Select(item =>
                {
                    var obj = item.Object;

                    // Jeśli obiekt implementuje IUserWithId, ustawiamy Id
                    if (obj is IUserWithId identifiable)
                    {
                        identifiable.Id = item.Key; // Ustawiamy Id na podstawie klucza z Firebase
                    }

                    return obj;
                }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching data: {ex.Message}");
                return new List<T>(); // W przypadku błędu zwróć pustą listę
            }
        }
        // dostaj dane o urzytkowniku po udanym logowaniu
        public async Task<User> GetUserDataAsync(string login)
        {
            // Pobierz użytkownika z Firebase na podstawie loginu
            var users = await firebaseClient.Child("User").OnceAsync<User>();
            var user = users.FirstOrDefault(u => u.Object.Login == login)?.Object;

            return user;
        }
        public async Task<bool> Login(string username, string password)
        {
            // Załóżmy, że masz bazę danych lub Firebase, w której zapisujesz użytkowników.
            var user = await GetUserDataAsync(username);

            if (user != null && user.Password == password)
            {
                // Zalogowano pomyślnie, przekazujemy dane użytkownika do AppShell.
                Application.Current.Properties["user"] = JsonConvert.SerializeObject(user);
                return true;
            }

            return false;
        }
        //login
        public async Task<User> LoginAsync(string login, string password)
        {
            try
            {
                // Pobieramy dane użytkowników z bazy danych
                var users = await firebaseClient.Child("User").OnceAsync<User>();

                // Przeszukujemy użytkowników w bazie
                var user = users.FirstOrDefault(u => u.Object.Login == login);

                if (user != null)
                {
                    // Sprawdzamy, czy hasło się zgadza
                    if (user.Object.Password == password)
                    {
                        // Zwracamy obiekt użytkownika po poprawnym logowaniu
                        return user.Object;
                    }
                }

                // Login lub hasło są niepoprawne
                return null;
            }
            catch (Exception ex)
            {
                // Obsługuje błędy
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }
        //search
        public async Task<List<User>> SearchByName(string name)
        {
            return (await firebaseClient.Child(nameof(User)).OnceAsync<User>()).Select(item => new User
            {
                Email = item.Object.Email,
                Name = item.Object.Name,
                Image = item.Object.Name,
                Id = item.Key
            }).Where(c=>c.Name.ToLower().Contains(name.ToLower())).ToList();
        }
        //get id
        public async Task<User>GetById(string id)
        {
            return (await firebaseClient.Child(nameof(User) + "/" + id).OnceSingleAsync<User>());
        }
        //update
        public async Task<bool> Update(User user)
        {
            await firebaseClient.Child(nameof(User) + "/" + user.Id).PutAsync(JsonConvert.SerializeObject(user));
            return true;
        }
        //delete
        public async Task<bool>Delete(string id)
        {
            await firebaseClient.Child(nameof(User) + "/" + id).DeleteAsync();
            return true;
        }
        //save to database
        public async Task<bool> SaveImageAsync(string base64Image, string imageName)
        {
            var imageData = new
            {
                Name = imageName,
                Image = base64Image
            };

            var data = await firebaseClient.Child("Images").PostAsync(JsonConvert.SerializeObject(imageData));

            return !string.IsNullOrEmpty(data.Key);
        }
    }
}
