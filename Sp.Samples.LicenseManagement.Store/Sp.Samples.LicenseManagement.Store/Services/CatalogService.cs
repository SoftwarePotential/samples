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

using Sp.Samples.LicenseManagement.Store.Models;
using System;
using System.Collections.Generic;

namespace Sp.Samples.LicenseManagement.Store.Services
{
	public class CatalogService
	{
		readonly ICatalogRepository _catalogRepository;

		public CatalogService( ICatalogRepository catalogRepository )
		{
			if ( catalogRepository == null )
				throw new ArgumentNullException( "catalogRepository" );

			_catalogRepository = catalogRepository;
		}

		public IEnumerable<CatalogEntry> ListAll()
		{
			var items = _catalogRepository.GetCatalogEntries();

			return items;
		}

		public void Add( CatalogEntry entry )
		{
			_catalogRepository.Add( entry );
		}

		internal CatalogEntry TryGet( int id )
		{
			CatalogEntry entry = new CatalogEntry();
			entry = _catalogRepository.TryGet( id );
			return entry;
		}

		internal void Delete( CatalogEntry entry )
		{
			_catalogRepository.Delete( entry );
		}

		internal void Update( CatalogEntry entry )
		{
			_catalogRepository.Update( entry );
		}
	}
}