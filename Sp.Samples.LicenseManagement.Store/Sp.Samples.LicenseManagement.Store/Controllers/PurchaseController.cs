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
using System.Configuration;
using System.Web.Mvc;
using System.Linq;

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
			var sqlRepository = new SqlPurchaseRecordRepository( ConfigurationManager.ConnectionStrings[ "StoreDbEntities" ].ConnectionString );
			_purchaseService = new PurchaseService( sqlRepository );

			var sqlCatalogRepository = new SqlCatalogRepository( ConfigurationManager.ConnectionStrings[ "StoreDbEntities" ].ConnectionString );
			_catalogService = new CatalogService( sqlCatalogRepository );

			_licensingService = new LicensingService( SoftwarePotentialConfiguration.File.ReadCredentials() );

			var sqlOrderItemRepository = new SqlOrderItemRepository( ConfigurationManager.ConnectionStrings[ "StoreDbEntities" ].ConnectionString );
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

		public ActionResult SelectedProduct( int id )
		{
			CatalogEntry entry = _catalogService.TryGet( id );
			CatalogEntryModel model = entry.ToViewModel();
			return View( model );
		}

		[HttpPost, ActionName( "SelectedProduct" )]
		public ActionResult SelectedProductConfirmed( int id, CatalogEntryModel model )
		{
			int quantity = model.Quantity;
			CatalogEntry entry = _catalogService.TryGet( id );
			model = entry.ToViewModel();
			string skuId = entry.SkuId;

			if ( ModelState.IsValid )
			{
				var purchaseRecord = _purchaseService.RecordPurchase( entry, quantity );

				for ( int i = 0; i < quantity; i++ )
				{
					License license = _licensingService.CreateLicenseFromSkuId( skuId );
					_orderItemService.RecordOrderItem( license, purchaseRecord );
				}

				var purchaseRecordModel = purchaseRecord.ToViewModel();

				return RedirectToAction( "ShowPurchasedInfo", new { id = purchaseRecordModel.Id } );
			}
			return View( model );
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
				LicensingBasis = model.LicensingBasis,
				OrderItemModels = (
					from oi in model.OrderItems
					select new OrderItemModel
					{
						Id = oi.Id,
						ActivationKey = oi.ActivationKey,
						LicenseId = oi.LicenseId,
						ExceptionDetails = oi.ExceptionDetails
					}).ToArray()
			};
			return purchaseRecordModel;
		}
	}
}
