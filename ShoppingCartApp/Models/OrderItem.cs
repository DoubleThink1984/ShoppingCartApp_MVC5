using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ShoppingCartApp.Models
{
    [Table("OrderItems")]
    public class OrderItem
    {
        [Key]
        [Display(Name = "Order Detail ID")]
        public int OrderDetailID { get; set; }
        
        [Required]
        [Display(Name ="Order ID")]
        public int OrderID { get; set; }

        [Required]
        [Display(Name = "Product ID")]
        public int ProductID { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal Cost { get; set; }
        
        public int Units { get; set; }

        [ForeignKey("ProductID")]
        public virtual Product OrderProduct { get; set; }
    }
}