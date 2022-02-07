using Bitango_.API;
using MonkeyCache.FileStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Bitango_.Models
{
    /// <summary>
    /// Информация контактов, соц сетей, описание ресторана
    /// </summary>
    public class AboutContainer
    {
        public AboutContainer(string descriptionRU, string descriptionEN, string email, string instagramURL, string phoneNumber,
            string websiteURL, string whatsappURL, long image)
        {
            this.image = image;
            LoadMainImage();

            PhoneNumber = phoneNumber;
            Email = email;
            InstagramURL = instagramURL;
            WhatsappURL = whatsappURL;
            WebsiteURL = websiteURL;
            DescriptionRU = descriptionRU;
            DescriptionEN = descriptionEN;
        }

        /// <summary>
        /// Асинхронная загрузка главной картинки на странице "О нас"
        /// </summary>
        private async void LoadMainImage()
        {
            Task<ImageSource> result = Task<ImageSource>.Factory.StartNew(() => ImageSource.FromUri(new Uri(DBConnect.GetImageURL(image.ToString()))));
            ImageSource = await result;
            Barrel.Current.Add("AboutContainer", this, expireIn: TimeSpan.FromDays(7)); //Add actual data to cache
        }
        /// <summary>
        /// Источник картинки на странице "О нас"
        /// </summary>
        public ImageSource ImageSource { get; set; }
        /// <summary>
        /// Номер телефона ресторана
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// Email ресторана
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// URL Instagram ресторана
        /// </summary>
        public string InstagramURL { get; set; }
        /// <summary>
        /// URL Whatsapp ресторана
        /// </summary>
        public string WhatsappURL { get; set; }
        /// <summary>
        /// Веб-сайт страница ресторана
        /// </summary>
        public string WebsiteURL { get; set; }
        /// <summary>
        /// Описание ресторана на русском
        /// </summary>
        public string DescriptionRU { get; set; }
        /// <summary>
        /// Описание ресторана на английском
        /// </summary>
        public string DescriptionEN { get; set; }
        /// <summary>
        /// ID картинки Flamelink на странице "О нас" 
        /// </summary>
        public long image { get; set; }
    }
}
