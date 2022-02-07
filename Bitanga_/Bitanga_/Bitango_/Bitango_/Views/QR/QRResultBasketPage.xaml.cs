using Bitango_.API;
using Bitango_.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing;
using Menu = Bitango_.Models.Menu;

namespace Bitango_.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QRResultBasketPage : ContentPage
    {
        Menu menu;
        List<Dish> scanned_dishes = new List<Dish>();
        Wallet user_wallet;
        int balance = 0;
        string name, ID, phone;
        public QRResultBasketPage()
        {
            InitializeComponent();
        }
        public QRResultBasketPage(ZXing.Result result)
        {
            InitializeComponent();

            ApiAccess.GetMenu();
            menu = ApiAccess.BaseMenu;

            string[] arr = result.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            ClientID_Label.Text = arr[0];
            ClientUsername_Label.Text = arr[1];
            ClientPhone_Label.Text = arr[2];
            ClientWalletID_Label.Text = arr[3];

            string dishes = "";

            for (int i = 4; i < arr.Length; i++) //Adding scanned dishes to list
            {
                Dish dish = FindDishByID(arr[i]);
                scanned_dishes.Add(dish);
                balance += dish.Price;
            }

            for (int i = 0; i < scanned_dishes.Count; i++)
            {
                dishes += scanned_dishes[i].Name + Environment.NewLine;
            }

            DishIDs_Label.Text = $"FOOD IDs: {dishes}";
            Format_Label.Text = $"Format: {result.BarcodeFormat}";

            user_wallet = new Wallet(balance, arr[3]);
            name = arr[1];
            ID = arr[0];
            phone = arr[2];
        }
        private Dish FindDishByID(string ID)
        {
            foreach (var category in menu)
            {
                foreach (Dish dish in category)
                {
                    if (dish.ID == ID)
                    {
                        return dish;
                    }
                }
            }
            return null;
        }
        private async void Order_Clicked(object sender, EventArgs e)
        {
            CurrentUser user = new CurrentUser(name, phone, ID, "example@gmail.com", "none", "none", user_wallet, "");
            Debug.WriteLine("ХУЙ2 " + ApiAccess.CreateOrder(user, scanned_dishes, menu));
            Debug.WriteLine("ХУЙ " + ApiAccess.WithdrawBalance(user, balance));
            await DisplayAlert("Заказ был успешно отправлен на кухню!", $"Было потрачено {balance} баллов.", "ОК");
            await Navigation.PopToRootAsync();
        }
    }
}