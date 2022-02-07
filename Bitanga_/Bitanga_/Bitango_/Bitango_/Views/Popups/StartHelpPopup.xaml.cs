using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Extensions;

namespace Bitango_.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartHelpPopup : PopupPage
    {
        INavigation navigation = Application.Current.MainPage.Navigation;
        private async void GoBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
        public StartHelpPopup()
        {
            InitializeComponent();
            this.BindingContext = this;
        }
        public ObservableCollection<Walkthrough> WalkthroughItems { get => Load(); }

        private ObservableCollection<Walkthrough> Load()
        {
            return new ObservableCollection<Walkthrough>(new[]
            {
                new Walkthrough
                {
                    Heading ="Введение",
                    Caption = "Данное приложение используется для посетителей ресторана Bitanga.",
                    Image = "CHANGEmountains.png"
                },

                new Walkthrough
                {
                    Heading ="Карта гостя",
                    Caption = "С помощью QR кода вы сможете получать очки за ваш заказ!",
                    Image = "CHANGECities.png"
                },

                new Walkthrough
                {
                    Heading ="Меню",
                    Caption = "В данном меню вы сможете с помощью накопленных очков получить блюда или напитки бесплатно!",
                    Image = "CHANGEAncient.png"
                }
            });
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            await navigation.PopPopupAsync();
        }
    }
    public class Walkthrough
    {
        public string Heading { get; set; }
        public string Caption { get; set; }
        public string Image { get; set; }
    }
}