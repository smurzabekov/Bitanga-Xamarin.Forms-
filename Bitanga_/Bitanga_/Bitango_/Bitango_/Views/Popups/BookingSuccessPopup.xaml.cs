using Rg.Plugins.Popup.Pages;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Bitango_.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BookingSuccessPopup : PopupPage
    {
        INavigation navigation = Application.Current.MainPage.Navigation;
        public BookingSuccessPopup()
        {
            InitializeComponent();
            CloseWhenBackgroundIsClicked = false;
        }
        /// <summary>
        /// Закрытие модалки успешного бронирования и переход на главную страницу
        /// </summary>
        private void Continue_Clicked(object sender, EventArgs e)
        {
            navigation.PopToRootAsync();
            navigation.PopModalAsync();
        }
    }
}