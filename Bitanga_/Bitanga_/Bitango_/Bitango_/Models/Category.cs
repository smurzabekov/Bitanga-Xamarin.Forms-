using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Bitango_.Models
{
    public class Category : Dish, IEnumerable<Dish>
    {
        List<Dish> products = new List<Dish>();

        public Category(string name, string description, string image, string iD, int price, string parentGroup) : base(name, description, image, iD, price, parentGroup)
        {
        }

        public void Add(Dish dish) => products.Add(dish);
        public IEnumerator<Dish> GetEnumerator()
        {
            return ((IEnumerable<Dish>)products).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<Dish>)products).GetEnumerator();
        }

        public int HowManyDishes => products.Count;
        public ObservableCollection<Dish> Dishes => new ObservableCollection<Dish>(products);
    }
}
