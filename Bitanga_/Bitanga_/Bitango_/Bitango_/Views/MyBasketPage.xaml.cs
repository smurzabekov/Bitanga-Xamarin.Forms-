using Bitango_.API;
using Bitango_.Files.Resources;
using Bitango_.Models;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Bitango_.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyBasketPage : ContentPage
    {
        ObservableCollection<Dish> dishes;
        int balance;
        public MyBasketPage()
        {
            InitializeComponent();
            Shell.SetNavBarHasShadow(this, false);

            dishes = Static.Basket.my_Orders;
            basket.ItemsSource = dishes;
            AllCosts.Text = Static.Basket.AllCosts.ToString();
            Amount.Text = Static.Basket.Amount.ToString();
        }
        private async void GoBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
        private async void Order_Clicked(object sender, EventArgs e)
        {
            ApiAccess.GetUserByID(Application.Current.Properties["ID"].ToString());
            balance = ApiAccess.current_user.wallet.Balance;
            Application.Current.Properties["Balance"] = balance;

            int allcost = Static.Basket.AllCosts;

            //if (balance < allcost)
            //{
            if (AppResources.Culture == new CultureInfo("ru"))
            {
                await DisplayAlert(AppResources.Error_, $"Недостаточно накопительных баллов для покупки. Требуется {allcost}. У вас {balance}", "OK");
            }
            else
            {
                await DisplayAlert(AppResources.Error_, $"Insufficient accumulated points to purchase. {allcost} is required. You have {balance}", "OK");
            }
            //return;
            //}
            await Navigation.PushPopupAsync(new QRBasketPopup(dishes), true);
        }
        private void Delete_Clicked(object sender, EventArgs e)
        {
            Button button = sender as Button;
            var dish = button.BindingContext as Dish;
            Static.Basket.my_Orders.Remove(dish);
            MessagingCenter.Send(this, "ChangeBasketIcon", Static.Basket.my_Orders.Count.ToString());
            AllCosts.Text = Static.Basket.AllCosts.ToString();
            Amount.Text = Static.Basket.Amount.ToString();
        }

        private void Plus_Clicked(object sender, EventArgs e)
        {
            Button button = sender as Button;
            var dish = button.BindingContext as Dish;
            for (int i = 0; i < Static.Basket.my_Orders.Count; i++)
            {
                if(dish.ID == Static.Basket.my_Orders[i].ID)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await DisplayAlert("Удачно добавлено", "+1", "OK");
                    });
                    Static.Basket.my_Orders[i].AmountInBasket += 1;                    
                    break;
                }
            }
        }

        private void Minus_Clicked(object sender, EventArgs e)
        {
            Button button = sender as Button;
            var dish = button.BindingContext as Dish;
            for (int i = 0; i < Static.Basket.my_Orders.Count; i++)
            {
                if (dish.ID == Static.Basket.my_Orders[i].ID)
                {
                    if(Static.Basket.my_Orders[i].AmountInBasket > 1)
                    {
                        Static.Basket.my_Orders[i].AmountInBasket -= 1;                       
                    }
                    else
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await DisplayAlert("Некуда уже убавлять!", "Данного блюда в корзине 1 штука", "OK");
                        });
                    }
                    break;
                }
            }
        }
    }
}