using Bitango_.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Bitango_.Static
{
    public class Basket
    {
        public static ObservableCollection<Dish> my_Orders = new ObservableCollection<Dish>();
        public static int Amount
        {
            get
            {
                return my_Orders.Count;
            }
        }
        public static int AllCosts
        {
            get
            {
                return my_Orders.Sum(x => x.Price);
            }
        }
    }
}
