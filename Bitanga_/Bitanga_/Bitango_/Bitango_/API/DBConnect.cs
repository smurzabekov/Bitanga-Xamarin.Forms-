using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Bitango_.Models;
using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Storage;
using MonkeyCache.FileStore;

namespace Bitango_.API
{
    /// <summary>
    /// Запись и чтение данных с Realtime Database
    /// </summary>
    public static class DBConnect
    {
        static FirebaseClient firebaseClient = new FirebaseClient("https://bitanga-ad98c.firebaseio.com/", new FirebaseOptions
        {
            AuthTokenAsyncFactory = () => Task.FromResult("yJtgbgGr9ApjOzTgmyCeowHeH7uotAsCV6vmIBYO")
        });

        static FirebaseStorage firebaseStorage = new FirebaseStorage("bitanga-ad98c.appspot.com");

        /// <summary>
        /// Добавление пользователя в Realtime Database
        /// </summary>
        /// <param name="user"></param>
        public static async void Add(User user)
        {
            var dino = await firebaseClient.Child("clients").PostAsync(user);
        }

        public static async void AddRole(string email, string role)
        {
            User user = GetUserByEmail(email);
            user.Role = role;
            await firebaseClient.Child("clients").Child(GetUserKey(email)).DeleteAsync();
            await firebaseClient.Child("clients").PostAsync(user);
        }

        /// <summary>
        /// Получение пользователя через email
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns>Пользователь</returns>
        static public User GetUserByEmail(string email)
        {
            var dino = Task.Run(() => firebaseClient.Child("clients").OrderByKey().OnceAsync<User>());
            dino.Wait();
            return dino.Result.Select(x => x.Object).Where(x => x.Email.Equals(email)).FirstOrDefault();
        }

        /// <summary>
        /// Получение роли пользователя через email
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns>Роль пользователя</returns>
        public static async Task<string> GetRole(string email)
        {
            var clients = Task.Run(() => firebaseClient.Child("clients").OrderByKey().OnceAsync<User>());
            clients.Wait();
            return clients.Result.Where(x => x.Object.Email.Equals(email)).First().Object.Role;
        }

        /// <summary>
        /// Удаления пользователя в Realtime Database
        /// </summary>
        /// <param name="user">Пользователь</param>
        public static async void Delete(CurrentUser user)
        {
            await firebaseClient.Child("clients").Child(GetUserKey(user.Email)).DeleteAsync();
        }

        /// <summary>
        /// Получение ключа пользователя через email
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns>Ключ пользователя</returns>
        static public string GetUserKey(string email)
        {
            var dino = Task.Run(() => firebaseClient.Child("clients").OrderByKey().OnceAsync<User>());
            dino.Wait();
            return dino.Result.Where(x => x.Object.Email.Equals(email)).FirstOrDefault().Key;
        }

        /// <summary>
        /// Получение ID пользователя через email
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns>ID пользователя</returns>
        static public string GetUserID(string email)
        {
            var dino = Task.Run(() => firebaseClient.Child("clients").OrderByKey().OnceAsync<User>());
            dino.Wait();
            return dino.Result.Select(x => x.Object).Where(x => x.Email.Equals(email)).FirstOrDefault().ID;
        }

        /// <summary>
        /// Проверка наличия пользователя с таким Email
        /// </summary>
        /// <param name="email">Email</param>
        static public bool IsEmailOnDB(string email)
        {
            var dino = Task.Run(() => firebaseClient.Child("clients").OrderByKey().OnceAsync<User>());
            dino.Wait();
            return dino.Result.Select(x => x.Object).Where(x => x.Email.Equals(email)).Count() != 0;
        }

        /// <summary>
        /// Проверка наличия пользователя с таким номером телефона
        /// </summary>
        /// <param name="phone">Номер телефона</param>
        static public bool IsPhoneOnDB(string phone)
        {
            var dino = Task.Run(() => firebaseClient.Child("clients").OrderByKey().OnceAsync<User>());
            dino.Wait();
            var linq = dino.Result.Select(x => x.Object).Where(x => x.Phone.Equals(phone));
            if (linq == null)
            {
                return false;
            }
            return linq.Count() != 0;
        }

        /// <summary>
        /// Получение всей информации страницы "О нас"
        /// </summary>
        /// <returns>Информация страницы "О нас"</returns>
        static public AboutContainer GetAboutPageData()
        {
            if (!Barrel.Current.IsExpired(key: "AboutContainer")) //Get about data from cache
            {
                return Barrel.Current.Get<AboutContainer>(key: "AboutContainer");
            }


            var data = Task.Run(() =>
                firebaseClient.Child("flamelink").Child("environments").Child("production").Child("content").Child("about_us").Child("en-US")
            .OrderByKey());
            data.Wait();
            var data_arr_str = data.Result.LimitToFirst(7).OnceAsync<string>().Result.ToArray();
            var data_arr_image = data.Result.StartAt("image").LimitToFirst(1).OnceAsync<long[]>().Result.ToArray();

            AboutContainer aboutContainer = new AboutContainer(data_arr_str[0].Object, data_arr_str[1].Object, data_arr_str[2].Object,
                data_arr_str[3].Object, data_arr_str[4].Object, data_arr_str[5].Object, data_arr_str[6].Object, data_arr_image[0].Object[0]);

            //Добавляю aboutContainer в кеш после получения картинки с сервера в конструкторе
            return aboutContainer;
        }

        /// <summary>
        /// Получение URL картинки из Firebase Storage
        /// </summary>
        /// <param name="imageID">Flamelink image ID</param>
        /// <returns>URL картинки</returns>
        static public string GetImageURL(string imageID)
        {
            var filenameQuery = Task.Run(() =>
                firebaseClient.Child("flamelink").Child("media").Child("files").Child(imageID)
            .OrderByKey().StartAt("file").LimitToFirst(1).OnceAsync<string>());
            filenameQuery.Wait();

            var filename = filenameQuery.Result.ToArray()[0].Object;
            System.Console.WriteLine("FILENAME: " + filename);

            var urlQuery = Task.Run(() =>
                 firebaseStorage.Child("flamelink").Child("media").Child(filename).GetDownloadUrlAsync()
             );
            urlQuery.Wait();

            return urlQuery.Result;
        }

        /// <summary>
        /// Получение актуальных акций с Realtime Database
        /// </summary>
        static public void GetPromotionList()
        {
            if (!Barrel.Current.IsExpired(key: "Promotions")) //Get promotions data from cache
            {
                ApiAccess.promotions = Barrel.Current.Get<List<Promotion>>(key: "Promotions");
                return;
            }

            List<Promotion> promotions = new List<Promotion>();
            var promotionQuery = Task.Run(() => firebaseClient.Child("flamelink").Child("environments").Child("production")
                .Child("content").Child("promotions").Child("en-US").OnceAsync<Promotion>());

            promotionQuery.Wait();

            foreach (var promotion in promotionQuery.Result)
            {
                promotions.Add(promotion.Object);
                promotion.Object.initializeImageSource();
            }

            Barrel.Current.Add("Promotions", promotions, expireIn: TimeSpan.FromDays(1)); //Add actual promotions data to cache

            ApiAccess.promotions = promotions;
        }
    }
}
