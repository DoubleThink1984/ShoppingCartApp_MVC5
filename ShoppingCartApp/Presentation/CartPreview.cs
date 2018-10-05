using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShoppingCartApp.Models;

namespace ShoppingCartApp.Presentation
{
    public class CartPreview
    {
        public int? CartItemCount { get; set; }

        public string CartTotal { get; set; }
    }
}