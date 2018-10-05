using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Web;
using ShoppingCartApp.Presentation;

namespace ShoppingCartApp.Models
{
    [Table("Orders")]
    public class Order
    {
        [Key]
        public int OrderID { get; set; }
        
        [Display(Name="Order Date")]
        public DateTime OrderDate { get; set; }

        [Required]
        [Display(Name = "Billing Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Billing Street Address")]
        public string Addr1 { get; set; }
        
        [Display(Name = "Address 2")]
        public string Addr2 { get; set; }

        [Required]
        [Display(Name = "Billing City")]
        public string City { get; set; }

        [Required]
        [Display(Name = "Billing State")]
        public string State { get; set; }

        [Required]
        [Display(Name = "Billing Postal Code")]
        [RegularExpression(@"^\d{5}(?:[-\s]\d{4})?$", ErrorMessage = "Enter A Valid Postal Code")]
        public string PostalCode { get; set; }

        [Required]
        [Display(Name = "Shipping Name")]
        public string ShipName { get; set; }

        [Required]
        [Display(Name = "Shipping Address")]
        public string ShipAddr1 { get; set; }

        [Display(Name = "Shipping Address 2")]
        public string ShipAddr2 { get; set; }

        [Required]
        [Display(Name = "Shipping City")]
        public string ShipCity { get; set; }

        [Required]
        [Display(Name = "Shipping State")]
        public string ShipState { get; set; }

        [Required]
        [Display(Name = "Shipping Postal Code")]
        [RegularExpression(@"^\d{5}(?:[-\s]\d{4})?$", ErrorMessage = "Enter A Valid Postal Code")]
        public string ShipPostalCode { get; set; }
        
        public string Details { get; set; }

        [Display(Name = "Parcel Post")]
        public bool ParcelPost { get; set; }

        [Display(Name = "Ship Via")]
        public string ShipVia { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string email { get; set; }

        [Display(Name = "Phone (555-555-5555)")]
        [DataType(DataType.PhoneNumber)]
        [Phone]
        [RegularExpression(@"^(\+\d{1,2}\s)?\(?\d{3}\)?[\s.-]\d{3}[\s.-]\d{4}$", ErrorMessage = "Enter A Valid Phone Number")]
        public string phone { get; set; }

        public bool archived { get; set; }

        public string Comments { get; set; }

        [Required]
        public string Country { get; set; }

        [Display(Name = "Shipping Country")]
        public string ShipCountry { get; set; }

        //public Guid CartID { get; set; }

        public decimal Shipping { get; set; }

        public decimal? Total { get; set; }
        
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Display(Name = "Card Number")]
        [DataType(DataType.CreditCard)]
        [NotMapped]
        public string CardNumber { get; set; }

        [Display(Name = "CVC (3 Didgits)")]
        [NotMapped]
        [StringLength(3, MinimumLength=3, ErrorMessage = "CVC must be a 3 didgit number")]
        public string CVC { get; set; }

        [Display(Name = "Cardholder Name")]
        [NotMapped]
        public string CardHolderName { get; set; }

        [Display(Name = "Expiration Date: MM/YYYY")]
        [RegularExpression(@"^(0[1-9]|1[0-2])/(19|2[0-1])\d{2}$", ErrorMessage = "Enter A Valid Phone Number")]
        [NotMapped]
        public string ExpirationDate { get; set; }

        [Display(Name = "Card Type")]
        [NotMapped]
        public string CardType { get; set; }
        public virtual List<OrderItem> OrderItems { get; set; }

        public void DecryptAndPopulateCreditCardFields(string cardDetails)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(cardDetails);
            string[] details = sb.ToString().Split('_');

            this.CardHolderName = details[0];
            this.CardType = details[1];
            this.CVC = details[2];
            this.ExpirationDate = details[3];
            this.CardNumber = Encryption.Decrypt(details[4]);
        }
    }
}