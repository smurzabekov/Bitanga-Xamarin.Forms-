using Bitango_.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Bitango_.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailsPage : ContentPage
    {
        public Dish SelectedDish;
        public DetailsPage()
        {
            InitializeComponent();
            Shell.SetNavBarIsVisible(this, false); //Скрыть верхнюю навигационную панель
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
        private async void GetInMyOrders_Clicked(object sender, EventArgs e)
        {
            Static.Basket.my_Orders.Add(SelectedDish);
            MessagingCenter.Send(this, "ChangeBasketIcon", Static.Basket.my_Orders.Count.ToString());
            await Navigation.PopToRootAsync();
        }
    }
}