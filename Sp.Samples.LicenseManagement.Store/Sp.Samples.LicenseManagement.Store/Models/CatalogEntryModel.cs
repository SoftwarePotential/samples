using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sp.Samples.LicenseManagement.Store.Models
{
	public class CatalogEntryModel
	{
		public int Id { get; set; }
		[ Display( Name="Product Name" ) ]
		[ Required ]
		public string ProductName { get; set; }
		[ Display( Name="Product Version" ) ]
		[ Required ]
		public string ProductVersion { get; set; }
		public string Blurb { get; set; }
		public string SkuId { get; set; }
		public Nullable<decimal> Price { get; set; }
	}
}