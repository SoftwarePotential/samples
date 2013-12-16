using System.ComponentModel.DataAnnotations;

namespace Sp.Samples.LicenseManagement.Store.Models
{
	public class OrderItemModel
	{
		public int OrderItemNo { get; set; }
		public int PurchaseRecordId { get; set; }
		[Display( Name = "Activation Key" )]
		public string ActivationKey { get; set; }
		[Display( Name = "License Id" )]
		public string LicenseId { get; set; }
		[Display( Name = "Exception Details" )]
		public string ExceptionDetails { get; set; }
	}
}