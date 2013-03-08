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
using Sp.Samples.LicenseManagement.Store.Services;
using System;

namespace Sp.Samples.LicenseManagement.Store.Models
{
	public class SqlCatalogRepository : ICatalogRepository
	{
		readonly Func<StoreDbEntities> _createStoreContext;

		public SqlCatalogRepository( Func<StoreDbEntities> createStoreContext )
		{
			_createStoreContext = createStoreContext;
		}

		public IEnumerable<CatalogEntry> GetCatalogEntries()
		{
			var catEntriesModel = new List<CatalogEntry>();
			using ( var context = _createStoreContext() )
				catEntriesModel = context.CatalogEntries.ToList();
			return catEntriesModel;
		}

		public CatalogEntry TryGet( int id )
		{
			using ( var context = _createStoreContext() )
			{
				CatalogEntry entry = context.CatalogEntries.Find( id );
				return entry;
			}
		}

		public void Add( CatalogEntry entry )
		{
			using ( var context = _createStoreContext() )
			{
				context.CatalogEntries.Add( entry );
				context.SaveChanges();
			}
		}

		public void Delete( CatalogEntry entry )
		{
			using ( var context = _createStoreContext() )
			{
				context.CatalogEntries.Attach( entry );
				context.CatalogEntries.Remove( entry );
				context.SaveChanges();
			}
		}

		public bool Update( CatalogEntry entry )
		{
			using ( var context = _createStoreContext() )
			{
				context.Entry( entry ).State = EntityState.Modified;
				context.SaveChanges();
			}
			return true;
		}
	}
}