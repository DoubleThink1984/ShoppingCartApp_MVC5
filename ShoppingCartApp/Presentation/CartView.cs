using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShoppingCartApp.Models;

namespace ShoppingCartApp.Presentation
{
    public class CartView
    {
        public List<Cart> CartEntries { get; set; }

        public List<Product> CartProducts { get; set; }

        public decimal Shipping { get; set; }

        public string CartTotal { get; set; }
    }
}