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

using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Sp.Samples.LicenseManagement.Store.Models
{
	public class SqlCatalogRepository : ICatalogRepository
	{
		readonly string _connString;

		public SqlCatalogRepository( string connString )
		{
			this._connString = connString;
		}

		private StoreModelContainer CreateContext()
		{
			return new StoreModelContainer();
		}

		public IEnumerable<CatalogEntry> GetCatalogEntries()
		{
			var catEntriesModel = new List<CatalogEntry>();
			using ( var context = CreateContext() )
				catEntriesModel = context.CatalogEntries.ToList();
			return catEntriesModel;
		}

		public CatalogEntry TryGet( int id )
		{
			using (var context = CreateContext(  ))
			{
				CatalogEntry entry = context.CatalogEntries.Find( id );
				return entry;
			}
		}

		public void Add( CatalogEntry entry )
		{
			using (var context = CreateContext(  ))
			{
				context.CatalogEntries.Add( entry );
				context.SaveChanges();
			}
		}

		public void Delete( CatalogEntry entry )
		{
			using (var context = CreateContext(  ))
			{
				context.CatalogEntries.Attach( entry );
				context.CatalogEntries.Remove( entry );
				context.SaveChanges();
			}
		}

		public bool Update( CatalogEntry entry )
		{
			using (var context = CreateContext(  ))
			{
				context.Entry( entry ).State = EntityState.Modified;
				context.SaveChanges();
			}
			return true;
		}
	}
}