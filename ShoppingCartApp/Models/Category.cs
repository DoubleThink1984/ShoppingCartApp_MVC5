using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using ShoppingCartApp.Presentation;

namespace ShoppingCartApp.Models
{
    [Table("Classes")]
    public class Category
    {
        [Key]
        [Display(Name = "Category ID")]
        public int ClassId { get; set; }

        [Required]
        [Display(Name = "Category Name")]
        public string ClassName { get; set; }

        [Display(Name = "Sub-Category ID")]
        public int? ParentClassID { get; set; }

        public string Description { get; set; }

        [ImageExtension(ErrorMessage = "Image must be PNG or JPG file")]
        public string ImageUrl { get; set; }

        public virtual List<Product> Products { get; set; }

        [NotMapped]
        public List<Category> Subcategories { get; set; }
    }
}