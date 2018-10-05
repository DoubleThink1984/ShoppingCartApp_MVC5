using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShoppingCartApp.Models;
using PagedList;

namespace ShoppingCartApp.Presentation
{
    public class BrowseView
    {
        public Category Category { get; set; }

        public PagedList.IPagedList<Product> Products { get; set; }
    }
}