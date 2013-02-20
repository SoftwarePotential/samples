using System.Collections.Generic;
using Sp.Samples.LicenseManagement.Store.Services;

namespace Sp.Samples.LicenseManagement.Store.Models
{
	public interface IPurchaseRecordRepository
	{
		IEnumerable<PurchaseRecord> GetPurchaseRecords();
		PurchaseRecord TryGet( int id );
		PurchaseRecord Add( PurchaseRecord purchaseRecord );
	}
}
