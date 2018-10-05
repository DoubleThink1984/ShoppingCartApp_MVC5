using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingCartApp.Presentation
{
    public class RemoveCartView
    {
        public string CartTotal { get; set; }
        public int CartQuantity { get; set; }
        public int ProductQuantity { get; set; }
        public int DeleteID { get; set; }

    }
}