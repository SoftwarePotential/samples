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
	public class PurchaseRecordModel
	{
		public int Id { get; set; }
		public Nullable<int> Quantity { get; set; }
		[ Display( Name="Catalog Entry" ) ]
		public int CatalogEntryId { get; set; }
		[ Display( Name="Product Name" ) ]
		public string ProductName { get; set; }
		[Display( Name = "Product Version" )]
		public string ProductVersion { get; set; }
		public string Description { get; set; }
		[Display( Name = "Licensing Basis" )]
		public string LicensingBasis { get; set; }
		[Display( Name = "License Id" )]
		public string LicenseId { get; set; }
		[Display( Name = "Activation Key" )]
		public string ActivationKey { get; set; }
	}
}