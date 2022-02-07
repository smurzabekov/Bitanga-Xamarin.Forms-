using Bitango_.Models;
using Bitango_.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Bitango_.ViewModel
{
    public class DishViewModel : BaseViewModel
    {
        public DishViewModel() { }
        private Dish selectedDish;
        public Dish SelectedDish
        {
            get { return selectedDish; }
            set
            {
                selectedDish = value;
                OnPropertyChanged();
            }
        }
        public string AmountBasket
        {
            get { return Static.Basket.Amount.ToString();  }
        } 
        public ICommand SelectionCommand => new Command(DisplayDish);
        private void DisplayDish()
        {
            if (selectedDish != null)
            {
                var viewModel = new DetailsViewModel { SelectedDish = selectedDish};
                var detailsPage = new DetailsPage { BindingContext = viewModel, SelectedDish = selectedDish };

                var navigation = Application.Current.MainPage.Navigation;
                SelectedDish = null;
                navigation.PushAsync(detailsPage, true);
            }
        }
    }
}
