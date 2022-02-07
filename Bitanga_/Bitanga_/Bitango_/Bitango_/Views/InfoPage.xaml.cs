using System;
using Xamarin.Forms;
using Bitango_.API;
using Xamarin.Essentials;
using Xamarin.Forms.Xaml;
using Bitango_.Files.Resources;
using System.Globalization;
using System.Threading.Tasks;

namespace Bitango_.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InfoPage : ContentPage
    {
        public InfoPage()
        {
            InitializeComponent();
            Shell.SetNavBarHasShadow(this, false); // Delete shadow undel TitleView

            if (ApiAccess.aboutContainer == null)
            {
                ApiAccess.aboutContainer = DBConnect.GetAboutPageData(); //Get actual data of about page
            }

            if (AppResources.Culture == new CultureInfo("ru"))
            {
                Description.Text = ApiAccess.aboutContainer.DescriptionRU;
            }
            else
            {
                Description.Text = ApiAccess.aboutContainer.DescriptionEN;
            }

            aboutImage.Source = ApiAccess.aboutContainer.ImageSource;
        }

        /// <summary>
        /// Открытие веб-страницы
        /// </summary>
        private async void Globe_Clicked(object sender, EventArgs e)
        {
            if (ApiAccess.aboutContainer.WebsiteURL != null)
            {
                await Launcher.OpenAsync(ApiAccess.aboutContainer.WebsiteURL);
            }
        }
        /// <summary>
        /// Открытие карты
        /// </summary>
        private async void Map_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MapPage());
        }
        /// <summary>
        /// Открытие номера телефона ресторана
        /// </summary>
        private void Phone_Clicked(object sender, EventArgs e)
        {
            PhoneDialer.Open(ApiAccess.aboutContainer.PhoneNumber);
        }
        /// <summary>
        /// Открытие Instagram
        /// </summary>
        private async void Instagram_Clicked(object sender, EventArgs e)
        {
            await Launcher.OpenAsync(ApiAccess.aboutContainer.InstagramURL);
        }
        /// <summary>
        /// Открытие Whatsapp
        /// </summary>
        private async void Whatsapp_Clicked(object sender, EventArgs e)
        {
            await Launcher.OpenAsync(ApiAccess.aboutContainer.WhatsappURL);
        }
    }
}