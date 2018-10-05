using ShoppingCartApp.Presentation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ShoppingCartApp.Models
{
    [Table("ProductImages")]
    public class ProductImages
    {
        [Key]
        [Display(Name = "Image ID")]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        
        [Display(Name = "Product ID")]
        public int ProductID { get; set; }
                
        [Display(Name = "Image Url")]
        public string Path { get; set; }
        
        public bool Featured { get; set; }
    }
}