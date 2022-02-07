using Bitango_.API;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Bitango_.Models
{
    /// <summary>
    /// Акция или новость ресторана
    /// </summary>
    public class Promotion
    {
        //public Promotion()
        //{
        //    ImageSource = ImageSource.FromUri(new Uri(ImageURL));
        //}

        /// <summary>
        /// Заголовок акции на русском
        /// </summary>
        public string TitleRU { get; set; }
        /// <summary>
        /// Заголовок акции на английском
        /// </summary>
        public string TitleEN { get; set; }
        /// <summary>
        /// Подзаголовок акции на русском
        /// </summary>
        public string SubtitleRU { get; set; }
        /// <summary>
        /// Подзаголовок акции на английском
        /// </summary>
        public string SubtitleEN { get; set; }
        /// <summary>
        /// Описание акции на русском
        /// </summary>
        public string DescriptionRU { get; set; }
        /// <summary>
        /// Описание акции на английском
        /// </summary>
        public string DescriptionEN { get; set; }
        /// <summary>
        /// Тип кнопки
        /// </summary>
        public string TypeOfButton { get; set; }
        /// <summary>
        /// URL сайта для перехода по кнопке
        /// </summary>
        public string WebsiteURL { get; set; }
        /// <summary>
        /// ID картинки Flamelink
        /// </summary>
        public long[] image { get; set; }

        /// <summary>
        /// Заголовок для вывода на главной странице
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Подзаголовок для вывода на главной странице
        /// </summary>
        public string Subtitle { get; set; }
        /// <summary>
        /// Описание для вывода на главной странице
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Источник для картинки акции
        /// </summary>
        public ImageSource ImageSource { get; set; }
        public string ImageURL { get; set; }
        public void initializeImageSource()
        {
            //ImageSource = ImageSource.FromUri(new Uri(DBConnect.GetImageURL(image[0].ToString())));
            ImageSource = ImageSource.FromUri(new Uri(ImageURL));
        }
    }
}
