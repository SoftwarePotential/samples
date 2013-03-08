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

using Sp.Samples.LicenseManagement.Store.Services;
using System.Collections.Generic;
using System.Linq;

namespace Sp.Samples.LicenseManagement.Store.Models
{
	public class SqlPurchaseRecordRepository:IPurchaseRecordRepository
	{
		string _connString;

		public SqlPurchaseRecordRepository( string connString )
		{
			_connString = connString;
		}

		private StoreDbEntities CreateContext()
		{
			return new StoreDbEntities();
		}

		public IEnumerable<PurchaseRecord> GetPurchaseRecords()
		{
			var recordsList = new List<PurchaseRecord>();
			using ( var context = CreateContext() )
				recordsList = context.PurchaseRecords
					.Include( "OrderItems" )
					.ToList();
			return recordsList;
		}

		public PurchaseRecord Add( PurchaseRecord record )
		{
			using (var context = CreateContext(  ) )
			{
				context.PurchaseRecords.Add( record );
				context.SaveChanges();
				return record;
			}
		}

		public PurchaseRecord TryGet( int id )
		{
			using (var context = CreateContext(  ) )
			{
				PurchaseRecord record = context.PurchaseRecords
					.Include( "OrderItems" )					
					.SingleOrDefault( r => r.Id == id );
				return record;
			}
		}
	}
}