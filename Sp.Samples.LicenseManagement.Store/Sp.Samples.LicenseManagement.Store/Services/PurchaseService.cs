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

using Sp.Samples.LicenseManagement.Store.LicenseManagementWS;
using Sp.Samples.LicenseManagement.Store.Models;
using System;
using System.Collections.Generic;

namespace Sp.Samples.LicenseManagement.Store.Services
{
	public class PurchaseService
	{
		readonly IPurchaseRecordRepository _purchaseRecordRepository;

		public PurchaseService( IPurchaseRecordRepository purchaseRecordRepository )
		{
			if ( purchaseRecordRepository == null )
				throw new ArgumentNullException( "purchaseRecordRepository" );
			_purchaseRecordRepository = purchaseRecordRepository;
		}

		public PurchaseRecord Add( PurchaseRecord record )
		{
			_purchaseRecordRepository.Add( record );
			return record;
		}

		public PurchaseRecord TryGet( int id )
		{
			PurchaseRecord record = new PurchaseRecord();
			record = _purchaseRecordRepository.TryGet( id );
			return record;
		}

		public IEnumerable<PurchaseRecord> GetPurchaseRecords()
		{
			var items = _purchaseRecordRepository.GetPurchaseRecords();

			return items;
		}

		public PurchaseRecord RecordPurchase( CatalogEntry entry, int quantity )
		{
			PurchaseRecord purchaseRecord = new PurchaseRecord() 
			{ 
				ProductName = entry.ProductName, 
				ProductVersion = entry.ProductVersion, 
				Description = entry.Blurb, 
				LicensingBasis = entry.LicensingBasis,
				Quantity = quantity
			};
						
			return Add( purchaseRecord );
		}
	}
}