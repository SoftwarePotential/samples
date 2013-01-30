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

using System.Web.Mvc;
using Sp.Samples.LicenseManagement.Store.Models;
using Sp.Samples.LicenseManagement.Store.Services;
using System.Configuration;

namespace Sp.Samples.LicenseManagement.Store.Controllers
{
	//The purpose of this Controller is to perform administrative functions on the catalog of products 
	//offered by the Software Vendor. Ordinarily these administrative tasks would be restricted to 
	//authorized administrators only. However, for the purposes of simplicity in this demonstration of 
	//Software Potential's Web APIS, no authorization restrictions have been included.

	public class CatalogController : Controller
	{
		readonly CatalogService _catalogService;

		public CatalogController()
		{
			var sqlRepository = new SqlCatalogRepository( ConfigurationManager.ConnectionStrings[ "StoreModelContainer" ].ConnectionString );
			_catalogService = new CatalogService( sqlRepository );
		}
		
		public ActionResult Index()
		{
			return View( _catalogService.ListAll() );
		}

		public ActionResult Buy()
		{
			return View( _catalogService.ListAll() );
		}
		
		public ActionResult Details( int id = 0 )
		{
			CatalogEntry catalogEntry = _catalogService.TryGet( id );
			if ( catalogEntry == null )
				return HttpNotFound();
			return View( catalogEntry );
		}
		
		public ActionResult Create()
		{
			return View();
		}
				
		[HttpPost]
		public ActionResult Create( CatalogEntry catalogEntry )
		{
			if ( ModelState.IsValid )
			{
				_catalogService.Add( catalogEntry );
				return RedirectToAction( "Index" );
			}

			return View( catalogEntry );
		}
				
		public ActionResult Edit( int id = 0 )
		{
			CatalogEntry catalogEntry = _catalogService.TryGet( id );
			if ( catalogEntry == null )
				return HttpNotFound();
			return View( catalogEntry );
		}

		[HttpPost]
		public ActionResult Edit( CatalogEntry catalogEntry )
		{
			if ( ModelState.IsValid )
			{
				_catalogService.Update( catalogEntry );
				return RedirectToAction( "Index" );
			}
			return View( catalogEntry );
		}
				
		public ActionResult Delete( int id = 0 )
		{
			CatalogEntry catalogEntry = _catalogService.TryGet( id );
			if ( catalogEntry == null )
				return HttpNotFound();
			return View( catalogEntry );
		}
				
		[HttpPost, ActionName( "Delete" )]
		public ActionResult DeleteConfirmed( int id )
		{
			CatalogEntry catalogEntry = _catalogService.TryGet( id );
			if ( catalogEntry != null )
				_catalogService.Delete( catalogEntry );
			return RedirectToAction( "Index" );
		}

		public ActionResult Purchase()
		{
			ViewBag.Message = "Page under construction.";

			return View();
		}
	}
}