using ShoppingCartApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingCartApp.Presentation
{
    public class ProductDetailsView
    {
        public Product Product { get; set; }

        public int? Quantity { get; set; }

        public bool InCart { get; set; }
    }
}