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
using Sp.Samples.LicenseManagement.Store.Services;
using System.Configuration;
using System.Web.Mvc;

namespace Sp.Samples.LicenseManagement.Store.Controllers
{
    public class PurchaseController : Controller
    {
		readonly PurchaseService _purchaseService;
        readonly CatalogService _catalogService;
		readonly LicensingService _licensingService;

		public PurchaseController()
		{
			var sqlRepository = new SqlPurchaseRecordRepository( ConfigurationManager.ConnectionStrings[ "StoreDbEntities" ].ConnectionString );
			_purchaseService = new PurchaseService( sqlRepository );

			var sqlCatalogRepository = new SqlCatalogRepository( ConfigurationManager.ConnectionStrings[ "StoreDbEntities" ].ConnectionString );
			_catalogService = new CatalogService( sqlCatalogRepository );

			_licensingService = new LicensingService( SoftwarePotentialConfiguration.File.ReadCredentials() );
		}

		public ActionResult Buy()
		{
			return View( _catalogService.ListAll() );
		}

		public ActionResult PurchaseDetails( int id = 0 )
		{
			PurchaseRecord purchaseRecord = _purchaseService.TryGet( id );
			PurchaseRecordModel purchaseRecordModel = purchaseRecord.ToViewModel();
			if ( purchaseRecord == null )
				return HttpNotFound();
			return View( purchaseRecordModel );
		}

		public ActionResult ListPurchases()
		{
			return View( _purchaseService.GetPurchaseRecords() );
		}

		public ActionResult SelectedProduct( int id )
		{
			CatalogEntry entry = _catalogService.TryGet( id );
			CatalogEntryModel model = entry.ToViewModel();
			return View( model );
		}

		[HttpPost, ActionName( "SelectedProduct" )]
		public ActionResult SelectedProductConfirmed( int id )
        {
			var itemDetails = _catalogService.TryGet( id );
			string skuId = itemDetails.SkuId;

			License license = _licensingService.CreateLicenseFromSkuId( skuId );

			var purchaseRecord = _purchaseService.RecordPurchase( itemDetails, license );

			var purchaseRecordModel = purchaseRecord.ToViewModel();

			return RedirectToAction( "ShowPurchasedInfo", new { id = purchaseRecordModel.Id } );
        }

		public ActionResult ShowPurchasedInfo( int id )
        {
			var purchaseRecord = _purchaseService.TryGet( id );
			var purchaseRecordModel = purchaseRecord.ToViewModel();
			return View( purchaseRecordModel );
        }
    }

	static class PurchaseRecordConversionExtensions
	{
		public static PurchaseRecordModel ToViewModel( this PurchaseRecord model )
		{
			PurchaseRecordModel purchaseRecordModel = new PurchaseRecordModel
			{
				Id = model.Id,
				Quantity = model.Quantity,
				CatalogEntryId = model.CatalogEntryId,
				ProductName = model.ProductName,
				ProductVersion = model.ProductVersion,
				Description = model.Description,
				ActivationKey = model.ActivationKey,
				LicenseId = model.LicenseId
			};
			return purchaseRecordModel;
		}

		public static PurchaseRecord ToServiceModel( this PurchaseRecordModel model )
		{
			PurchaseRecord record = new PurchaseRecord
			{
				Id = model.Id,
				Quantity = model.Quantity,
				CatalogEntryId = model.CatalogEntryId,
				ProductName = model.ProductName,
				ProductVersion = model.ProductVersion,
				Description = model.Description,
				ActivationKey = model.ActivationKey,
				LicenseId = model.LicenseId
			};
			return record;
		}
	}
}
