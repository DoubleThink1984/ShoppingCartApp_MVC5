﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ShoppingCartApp
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class wildman1Entities1 : DbContext
    {
        public wildman1Entities1()
            : base("name=wildman1Entities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
    
        public virtual int Delete_All_Product_Images(Nullable<int> productID)
        {
            var productIDParameter = productID.HasValue ?
                new ObjectParameter("ProductID", productID) :
                new ObjectParameter("ProductID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Delete_All_Product_Images", productIDParameter);
        }
    
        public virtual int Make_Featured_Image(Nullable<int> productID, Nullable<int> iD)
        {
            var productIDParameter = productID.HasValue ?
                new ObjectParameter("ProductID", productID) :
                new ObjectParameter("ProductID", typeof(int));
    
            var iDParameter = iD.HasValue ?
                new ObjectParameter("ID", iD) :
                new ObjectParameter("ID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Make_Featured_Image", productIDParameter, iDParameter);
        }
    
        public virtual int delete_abandoned_carts()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("delete_abandoned_carts");
        }
    }
}
