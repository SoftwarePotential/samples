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
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
		[DataType( DataType.MultilineText )]
		public string Blurb { get; set; }
		[Display( Name = "SKU Id" )]
		[Required( ErrorMessage = "A valid SKU Id is required." )]
		public string SkuId { get; set; }
		public Nullable<decimal> Price { get; set; }
		[ Display( Name="Licensing Basis" ) ]
		[Required( ErrorMessage = "You must select a Licensing Basis." )]
		public string LicensingBasis { get; set; }

		public List<string> LicensingBases { get; set; }

		[ Range( 1, Int32.MaxValue, ErrorMessage="Value must be greater than zero." ) ]
		public int Quantity { get; set; }
	}
}