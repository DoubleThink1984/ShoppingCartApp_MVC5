using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ShoppingCartApp.Presentation;

namespace ShoppingCartApp.Models
{
    [Table("Products")]
    public class Product
    {
        [Key]
        [Display(Name = "Product ID")]
        public int ProductID { get; set; }

        [Required]
        [Display(Name = "Product Code")]
        public string ProductCode { get; set; }

        [Required]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Short Description")]
        public string ShortDescription { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Long Description")]
        public string LongDescription { get; set; }
        
        public int? Quantity { get; set; }

        public decimal? MSRP { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Display(Name = "Sale Price")]
        public decimal? WildmanPrice { get; set; }

        [Required]
        [Display(Name = "Length in Inches")]
        public decimal? Length { get; set; }

        [Required]
        [Display(Name = "Width in Inches")]
        public decimal? Width { get; set; }

        [Required]
        [Display(Name = "Height in Inches")]
        public decimal? Height { get; set; }

        [Display(Name = "Parcel Post")]
        public bool ParcelPost { get; set; }

        [Display(Name = "Inactive Product")]
        public bool InactiveProduct { get; set; }

        [Display(Name = "No Back Orders")]
        public bool NoBackOrders { get; set; }

        [Display(Name = "Limit One")]
        public bool LimitOne { get; set; }
        
        public bool New { get; set; }

        [Display(Name = "Keep Inventory")]
        public bool KeepInventory { get; set; }

        [Display(Name = "Special Order")]
        public bool SpecialOrder { get; set; }

        [Display(Name = "Featured Product")]
        public bool FeaturedProduct { get; set; }

        [Display(Name = "Category ID")]
        public int ClassID { get; set; }

        public virtual List<ProductImages> ProductImages { get; set; }
    }
}