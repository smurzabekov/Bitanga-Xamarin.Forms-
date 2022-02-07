using Bitango_.Models;
using Bitango_.Views.GiftPageItems;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Bitango_.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GiftPage : ContentPage
    {        
        public GiftPage()
        {
            InitializeComponent();

            Shell.SetNavBarHasShadow(this, false); // Delete shadow undel TitleView

            Amount.Text = Static.Basket.Amount.ToString();
            MessagingCenter.Subscribe<DetailsPage, string>(this, "ChangeBasketIcon", (sender, e) =>
            {
                Amount.Text = e;
            });
        }
        /// <summary>
        /// Открытие корзины
        /// </summary>
        private async void Basket_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MyBasketPage());
        }
    }
}