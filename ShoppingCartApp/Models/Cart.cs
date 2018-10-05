using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ShoppingCartApp.Models
{
    [Table("ShoppingCart")]
    public class Cart
    { 
        [Key]
        public int ID { get; set; }

        [Required]
        [Display(Name = "Cart ID")]
        public String CartID { get; set; }

        [Display(Name = "Registered Cart ID")]
        public int RegisteredID { get; set; }

        [Required]
        public int ProductID { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        public bool Abandoned { get; set; }

        public int Units { get; set; }

        public Product Product { get; set; }
    }
}