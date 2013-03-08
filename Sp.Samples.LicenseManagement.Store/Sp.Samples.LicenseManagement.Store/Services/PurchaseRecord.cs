//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Sp.Samples.LicenseManagement.Store.Services
{
    using System;
    using System.Collections.Generic;
    
    public partial class PurchaseRecord
    {
        public PurchaseRecord()
        {
            this.OrderItems = new HashSet<OrderItem>();
        }
    
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int CatalogEntryId { get; set; }
        public string ProductName { get; set; }
        public string ProductVersion { get; set; }
        public string Description { get; set; }
        public string LicenseId { get; set; }
        public string ActivationKey { get; set; }
        public string LicensingBasis { get; set; }
    
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
