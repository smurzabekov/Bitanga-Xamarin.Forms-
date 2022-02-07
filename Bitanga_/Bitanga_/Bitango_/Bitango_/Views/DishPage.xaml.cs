using Bitango_.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Bitango_.Views.GiftPageItems
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DishPage : ContentPage
    {
        Category item;
        public DishPage()
        {
            InitializeComponent();

            Shell.SetNavBarHasShadow(this, false); // Delete shadow undel TitleView
            MessagingCenter.Subscribe<DetailsPage, string>(this, "ChangeBasketIcon", (sender, e) =>
            {
                Amount.Text = e;
            });
        }
        public DishPage(Category new_item)
        {
            item = new_item;
            InitializeComponent();

            Shell.SetNavBarHasShadow(this, false); // Delete shadow undel TitleView
            MessagingCenter.Subscribe<DetailsPage, string>(this, "ChangeBasketIcon", (sender, e) =>
            {
                Amount.Text = e;
            });
            MessagingCenter.Subscribe<DishPage, string>(this, "ChangeBasketIcon", (sender, e) =>
            {
                Amount.Text = e;
            });
            MessagingCenter.Subscribe<MyBasketPage, string>(this, "ChangeBasketIcon", (sender, e) =>
            {
                Amount.Text = e;
            });
            main_label.Text = item.Name;
            dishesList.ItemsSource = item.Dishes;
            //Amount.Text = Static.Basket.Amount.ToString();
        }

        private async void GoBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void Basket_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MyBasketPage());
        }
        private void InBasket_Clicked(object sender, EventArgs e)
        {
            Button button = sender as Button;
            var dish = button.BindingContext as Dish;
            bool isthereDish = false;
            for (int i = 0; i < Static.Basket.my_Orders.Count; i++)
            {
                if (Static.Basket.my_Orders[i] == dish)
                {
                    isthereDish = true;
                    break;
                }
            }
            if (!isthereDish)
            {
                Static.Basket.my_Orders.Add(dish);
                MessagingCenter.Send(this, "ChangeBasketIcon", Static.Basket.my_Orders.Count.ToString());
            }

        }
    }
}