using System;
using System.Collections.Generic;
using System.Text;

namespace Bitango_.Models
{
    public class Dish
    {
        public Dish(string name, string description,string image, string iD, int price, string parentGroup)
        { 
            Description = description;
            ID = iD;
            Price = price;
            ParentGroup = parentGroup;
            Name = name;
            AmountInBasket = 1;

            if (image != "")
                Image = image;
            else
                Image = @"https://png.pngtree.com/png-vector/20190926/ourlarge/pngtree-dish-icon-isolated-on-abstract-background-png-image_1742601.jpg";
        }
        public string Description { get; set; }
        public string ID { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public int Price { get; set; }
        public int AmountInBasket { get; set; }
        public string ParentGroup { get; set; }

       
    }
}
