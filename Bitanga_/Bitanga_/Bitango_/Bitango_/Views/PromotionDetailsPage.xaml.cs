using Bitango_.API;
using Bitango_.Files.Resources;
using Bitango_.Models;
using Plugin.Share;
using Plugin.Share.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Bitango_.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PromotionDetailsPage : ContentPage
    {
        public Promotion selectedPromotion;
        public PromotionDetailsPage(Promotion selectedPromotion)
        {
            this.selectedPromotion = selectedPromotion;
            InitializeComponent();
            Shell.SetNavBarIsVisible(this, false); //Скрыть верхнюю навигационную панель
            SetButton();
        }

        /// <summary>
        /// Задание функционала и текста кнопки у акции
        /// </summary>
        private void SetButton()
        {
            if (selectedPromotion != null)
            {
                switch (selectedPromotion.TypeOfButton)
                {
                    case "booking":
                        button.Clicked += Book_Clicked;
                        button.Text += AppResources.Book.ToUpper();
                        break;
                    case "instagram":
                        button.Clicked += Instagram_Clicked;
                        button.Text += AppResources.Open.ToUpper() + "INSTAGRAM";
                        break;
                    case "whatsapp":
                        button.Clicked += Whatsapp_Clicked;
                        button.Text += AppResources.Open.ToUpper() + "WHATSAPP";
                        break;
                    case "website":
                        button.Clicked += Website_Clicked;
                        button.Text += AppResources.Open.ToUpper();
                        break;
                    default:
                        button_stackLayout.IsVisible = false;
                        break;
                }
            }

        }
        private void GoBack_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            DetailsView.TranslationY = 600;
            DetailsView.TranslateTo(0, 0, 500, Easing.SinInOut);
        }
        /// <summary>
        /// Открытие бронирования
        /// </summary>
        private async void Book_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new BookPage());
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
        /// <summary>
        /// Открытие веб-страницы
        /// </summary>
        private async void Website_Clicked(object sender, EventArgs e)
        {
            await Launcher.OpenAsync(selectedPromotion.WebsiteURL);
        }
        /// <summary>
        /// Кнопка Поделиться
        /// </summary>
        private void Share_Clicked(object sender, EventArgs e)
        {
            CrossShare.Current.Share(new ShareMessage
            {
                Title = "Message",
                Text = "fioeshfsiohfskuiefhesjkfjhsuelilfhsefhjkfgesjfiegsf"
            });
        }
    }
}