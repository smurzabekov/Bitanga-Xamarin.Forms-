using Bitango_.API;
using Plugin.Connectivity;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace Bitango_.Views
{
    /// <summary>
    /// Модалка "Отсутсвует соединение с интернетом" на странице аутентификации
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NoInternetLoginPopup : PopupPage
    {
        public NoInternetLoginPopup()
        {
            InitializeComponent();
            CloseWhenBackgroundIsClicked = false;
        }
        private async void Continue_Clicked(object sender, EventArgs e)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                var navigation = Application.Current.MainPage.Navigation;

                DBConnect.GetPromotionList(); //Get actual data of promotions
                ApiAccess.aboutContainer = DBConnect.GetAboutPageData(); //Get actual data of about page

                await navigation.PopModalAsync();
            }
        }
    }
}