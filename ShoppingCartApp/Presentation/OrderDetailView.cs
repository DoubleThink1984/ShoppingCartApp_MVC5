using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShoppingCartApp.Models;

namespace ShoppingCartApp.Presentation
{
    public class OrderDetailView
    {
        public Order Order { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}