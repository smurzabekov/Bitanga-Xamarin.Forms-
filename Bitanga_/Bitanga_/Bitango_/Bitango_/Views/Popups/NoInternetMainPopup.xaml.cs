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
    /// Модалка "Отсутсвует соединение с интернетом" на главной странице
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NoInternetMainPopup : PopupPage
    {
        MainShellPage mainShellPage;
        public NoInternetMainPopup()
        {
            InitializeComponent();
            CloseWhenBackgroundIsClicked = false;
        }
        public NoInternetMainPopup(MainShellPage mainShellPage)
        {
            InitializeComponent();
            CloseWhenBackgroundIsClicked = false;

            this.mainShellPage = mainShellPage;
        }
        private async void Continue_Clicked(object sender, EventArgs e)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                var navigation = Application.Current.MainPage.Navigation;
                if (mainShellPage != null) //Работает в случае если во время инициализации страницы отсутсвует интернет
                {
                    App.Current.MainPage = new MainPage();
                }
                await navigation.PopModalAsync();
            }
        }
    }
}