﻿using BibliotekaPro.Models;
using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotekaPro
{
    public class Firebase
    {
        FirebaseClient firebaseClient = new FirebaseClient("https://bibliotekapro-eeddd-default-rtdb.firebaseio.com/");

        //add 
        //typ generyczny <T>
        public async Task<bool> Save<T>(T obj)
        {
            var collectionName = typeof(T).Name;
            var data = await firebaseClient.Child(collectionName).PostAsync(JsonConvert.SerializeObject(obj));

            return !string.IsNullOrEmpty(data.Key);
        }
        //save /get all
        public async Task<List<T>> GetAll<T>() where T : class, new()
        {
            return (await firebaseClient.Child(typeof(T).Name).OnceAsync<T>()).Select(item =>
            {
                var obj = item.Object;
                // Jeśli obiekt ma właściwość Id, ustaw ją ręcznie
                if (obj is IUserWithId identifiable)
                {
                    identifiable.Id = item.Key;
                }
                return obj;
            }).ToList();
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
    }
}
