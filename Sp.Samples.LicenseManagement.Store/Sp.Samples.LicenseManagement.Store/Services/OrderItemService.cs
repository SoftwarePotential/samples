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

namespace Sp.Samples.LicenseManagement.Store.Services
{
	public class OrderItemService
	{
		readonly IOrderItemRepository _orderItemRepository;

		public OrderItemService( IOrderItemRepository orderItemRepository )
		{
			if ( orderItemRepository == null )
				throw new ArgumentNullException( "orderItemRepository" );
			_orderItemRepository = orderItemRepository;
		}

		public OrderItem RecordOrderItem( License license, PurchaseRecord purchaseRecord )
		{
			OrderItem item = new OrderItem() 
			{ 
				PurchaseRecordId = purchaseRecord.Id,
				ActivationKey = license.ActivationKey,
				LicenseId = license.LicenseId 
			};

			_orderItemRepository.Add( item );
			return item;
		}
	}
}

