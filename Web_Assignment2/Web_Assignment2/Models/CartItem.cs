using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_Assignment2.Models
{
    public class CartItem
    {
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public int UnitAmount { get; set; }
        public int SubTotal { get { return Quantity * UnitAmount; } }
    }
}