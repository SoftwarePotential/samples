/*
 * Copyright 2013 (c) Inish Technology Ventures Limited.  All rights reserved.
 * 
 * This code is licensed under the BSD 3-Clause License included with this source
 * 
 * ALSO SEE: https://github.com/SoftwarePotential/samples/wiki/License
 * 
 */

// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.

using System;
using System.ComponentModel.DataAnnotations;

namespace Sp.Samples.LicenseManagement.Store.Models
{
	public class SelectedProductModel
	{
		public int Id { get; set; }
		public string Blurb { get; set; }
		[ Display( Name="Licensing Basis" ) ]
		public string LicensingBasis { get; set; }
		public Nullable<decimal> Price { get; set; }
		[Display( Name = "Product Name" )]
		public string ProductName { get; set; }
		[Display( Name = "Product Version" )]
		public string ProductVersion { get; set; }
		[Display( Name = "SKU Id" )]
		public string SkuId { get; set; }
		[Range( 1, Int32.MaxValue, ErrorMessage = "Value must be greater than zero." )]
		public int Quantity { get; set; }
	}
}

