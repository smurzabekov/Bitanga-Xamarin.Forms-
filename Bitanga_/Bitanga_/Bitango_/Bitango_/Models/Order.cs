using System;
using System.Collections.Generic;
using System.Text;

namespace Bitango_.Models
{
    public class Order
    {
        public int ID { get; set; }
        public uint TotalAmount { get; set; }
        public uint Bonuses { get; set; }
        public uint Quantity { get; set; }
        public DateTime DateOfIssue { get; set; }
    }
}
