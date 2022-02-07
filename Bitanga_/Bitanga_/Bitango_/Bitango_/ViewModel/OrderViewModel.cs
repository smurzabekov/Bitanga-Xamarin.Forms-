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
    public class OrderViewModel : BaseViewModel
    {
        ObservableCollection<Order> orders;
        public OrderViewModel()
        {
            orders = GetOrders();
        }
        public ObservableCollection<Order> Orders
        {
            get { return orders; }
            set
            {
                orders = value;
                OnPropertyChanged();
            }
        }
        private Order selectedOrder;
        public Order SelectedOrder
        {
            get { return selectedOrder; }
            set
            {
                selectedOrder = value;
                OnPropertyChanged();
            }
        }

        public ICommand SelectionCommand => new Command(DisplayOrder);

        private void DisplayOrder()
        {
            if (selectedOrder != null)
            {
                var viewModel = new DetailsViewModel { SelectedOrder = selectedOrder, Orders = orders };
                var detailsPage = new OrderPage { BindingContext = viewModel };

                var navigation = Application.Current.MainPage.Navigation;
                SelectedOrder = null;
                navigation.PushAsync(detailsPage, true);
            }
        }
        private ObservableCollection<Order> GetOrders()
        {
            return new ObservableCollection<Order>
            {
                new Order { ID=1947034, Bonuses=1100, DateOfIssue=DateTime.MaxValue, TotalAmount=11000, Quantity=3},
                new Order { ID=1947035, Bonuses=1200, DateOfIssue=DateTime.MaxValue, TotalAmount=12000, Quantity=6},
                new Order { ID=1947036, Bonuses=1300, DateOfIssue=DateTime.MaxValue, TotalAmount=13000, Quantity=9},
                new Order { ID=1947037, Bonuses=1400, DateOfIssue=DateTime.MaxValue, TotalAmount=14000, Quantity=12}

            };
        }
    }
}
