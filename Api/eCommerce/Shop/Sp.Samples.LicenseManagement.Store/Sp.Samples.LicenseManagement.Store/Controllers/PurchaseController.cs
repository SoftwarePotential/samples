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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Sp.Samples.LicenseManagement.Store.Controllers
{
	public class PurchaseController : Controller
	{
		readonly PurchaseService _purchaseService;
		readonly CatalogService _catalogService;
		readonly LicensingService _licensingService;
		readonly OrderItemService _orderItemService;

		public PurchaseController()
		{
			Func<StoreDbEntities> createStoreContext = (  )=> new StoreDbEntities(  );
			var sqlRepository = new SqlPurchaseRecordRepository( createStoreContext );
			_purchaseService = new PurchaseService( sqlRepository );

			var sqlCatalogRepository = new SqlCatalogRepository( createStoreContext );
			_catalogService = new CatalogService( sqlCatalogRepository );

			_licensingService = new LicensingService( SoftwarePotentialConfiguration.File.ReadCredentials() );

			var sqlOrderItemRepository = new SqlOrderItemRepository( createStoreContext );
			_orderItemService = new OrderItemService( sqlOrderItemRepository );
		}

		public ActionResult Buy()
		{
			List<CatalogEntryModel> catalogEntryModels = new List<CatalogEntryModel>();

			foreach ( CatalogEntry entry in _catalogService.ListAll() )
			{
				var catalogEntryViewModel = entry.ToViewModel();
				if ( String.IsNullOrEmpty( catalogEntryViewModel.LicensingBasis ) )
					catalogEntryViewModel.LicensingBasis = "N/A";
				catalogEntryModels.Add( catalogEntryViewModel );
			}
			return View( catalogEntryModels );
		}		

		public ActionResult SelectedProduct( int id )
		{
			CatalogEntry entry = _catalogService.TryGet( id );
			SelectedProductModel model = entry.ToSelectedProductModel();
			return View( model );
		}

		[HttpPost, ActionName( "SelectedProduct" )]
		public ActionResult SelectedProductConfirmed( SelectedProductModel model )
		{
			if ( !ModelState.IsValid )
			{
				return SelectedProduct(model.Id);
			}

			var entry = _catalogService.TryGet( model.Id );
			var purchaseRecord = _purchaseService.RecordPurchase( entry, model.Quantity );

			for ( int i = 0; i < model.Quantity; i++ )
			{
				License license = _licensingService.CreateLicenseFromSkuId( entry.SkuId );
				_orderItemService.RecordOrderItem( license, purchaseRecord, i + 1 );
			}

			return RedirectToAction( "ShowPurchasedInfo", new { id = purchaseRecord.Id } );
		}

		public ActionResult ShowPurchasedInfo( int id )
		{
			var purchaseRecord = _purchaseService.TryGet( id );
			var purchaseRecordModel = purchaseRecord.ToViewModel();
			return View( purchaseRecordModel );
		}

		public ActionResult PurchaseDetails( int id = 0 )
		{
			PurchaseRecord purchaseRecord = _purchaseService.TryGet( id );
			if ( purchaseRecord == null )
				return HttpNotFound();
			PurchaseRecordModel purchaseRecordModel = purchaseRecord.ToViewModel();
			return View( purchaseRecordModel );
		}

		public ActionResult ListPurchases()
		{
			IEnumerable<PurchaseRecord> purchaseRecords = _purchaseService.GetPurchaseRecords();
			List<PurchaseRecordModel> purchaseRecordModels = new List<PurchaseRecordModel>();

			foreach ( PurchaseRecord record in purchaseRecords )
			{
				purchaseRecordModels.Add( record.ToViewModel() );
			}

			return View( purchaseRecordModels );
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
				ProductName = model.ProductName,
				ProductVersion = model.ProductVersion,
				Description = model.Description,
				LicensingBasis = model.LicensingBasis,
				OrderItemModels = (
					from oi in model.OrderItems
					select new OrderItemModel
					{
						OrderItemNo = oi.OrderItemNo,
						PurchaseRecordId = oi.PurchaseRecordId,
						ActivationKey = oi.ActivationKey,
						LicenseId = oi.LicenseId
					}).ToArray()
			};
			return purchaseRecordModel;
		}
	}
	static class CatalogEntryToSelectedProductConversionExtension
	{
		public static SelectedProductModel ToSelectedProductModel( this CatalogEntry entry )
		{
			SelectedProductModel selectedProductModel = new SelectedProductModel
			{
				Id = entry.Id,
				Blurb = entry.Blurb,
				LicensingBasis = entry.LicensingBasis,
				Price = entry.Price,
				ProductName = entry.ProductName,
				ProductVersion = entry.ProductVersion,
				SkuId = entry.SkuId
			};
			return selectedProductModel;
		}
	}
}
