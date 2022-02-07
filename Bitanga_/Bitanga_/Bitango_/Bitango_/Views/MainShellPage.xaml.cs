using Bitango_.API;
using Bitango_.ViewModel;
using Plugin.Connectivity;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Diagnostics;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Bitango_.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainShellPage : ContentPage
    {
        private string connection;
        public MainShellPage()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();


            try
            {
                InitializeComponent();

                MessagingCenter.Subscribe<NoInternetMainPopup, string>(this, "ChangeBalance", (sender, e) =>
                {
                    Balance.Text = e;
                });

                if (CrossConnectivity.Current.IsConnected)
                {
                    if (ApiAccess.promotions == null)
                    {
                        DBConnect.GetPromotionList(); //Get actual data of promotions
                    }
                    if (ApiAccess.aboutContainer == null)
                    {
                        ApiAccess.aboutContainer = DBConnect.GetAboutPageData(); //Get actual data of about page
                    }

                    ApiAccess.GetUserByID(Application.Current.Properties["ID"].ToString());
                    //decimal balance = ApiAccess.current_user.wallet.Balance;  //Get balance of user from iiko
                    //Application.Current.Properties["Balance"] = balance;
                    //Balance.Text = $"{balance} @ ";

                    Application.Current.Properties["Balance"] = 228;
                    Balance.Text = $"228 @ ";

                    Application.Current.Properties["Username"] = ApiAccess.current_user.Name;
                    Application.Current.Properties["Phone"] = ApiAccess.current_user.Phone;
                    Application.Current.Properties["Birthday"] = ApiAccess.current_user.Birthday;

                    if (ApiAccess.current_user.Role == "admin" || ApiAccess.current_user.Role == "employ")
                    {
                        Scanner.IsEnabled = true;
                    }

                    this.BindingContext = new MainViewModel();
                   
                    StyleUI();
                }
                else
                {
                    Navigation.PushModalAsync(new NoInternetMainPopup(this));
                }
                
            }
            catch (Exception e)
            {
                DisplayAlert("Error", e.Message, "OK");
            }

            stopwatch.Stop();
            DisplayAlert("Error", "TIME OF WORKING MAIN: " + stopwatch.ElapsedMilliseconds, "OK");
        }
        /// <summary>
        /// Работа с пользовательским интерфейсом
        /// </summary>
        public void StyleUI()
        {
            Shell.SetNavBarHasShadow(this, false); // Delete shadow undel TitleView
            menuList.WidthRequest = ApiAccess.promotions.Count * 305; //Ширина CollectionView для акций
        }

        /// <summary>
        /// Internet соединение
        /// </summary>
        public string Connection {
            get {
                return connection;
            }
            set {
                connection = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Карта гостя (QR)
        /// </summary>
        private async void GuestCard_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushPopupAsync(new QRGuestCardPopup(), true);
        }
        /// <summary>
        /// Бронирование
        /// </summary>
        private async void Book_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new BookPage());
        }
        /// <summary>
        /// Открытие номера телефона ресторана
        /// </summary>
        private void Phone_Clicked(object sender, EventArgs e)
        {
            PhoneDialer.Open(ApiAccess.aboutContainer.PhoneNumber);
        }
        /// <summary>
        /// Открытие сканера для администратора
        /// </summary>
        private async void Scanner_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new QRScannerBasket());
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
        private async void Google_Clicked(object sender, EventArgs e)
        {
            await Launcher.OpenAsync(ApiAccess.aboutContainer.WhatsappURL);
        }
    }
}
