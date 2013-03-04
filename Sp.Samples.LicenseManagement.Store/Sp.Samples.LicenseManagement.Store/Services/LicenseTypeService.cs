using System.Collections.Generic;

namespace Sp.Samples.LicenseManagement.Store.Services
{
	public class LicenseTypeService
	{
		public static List<string> GetLicenseTypes()
		{
			List<string> licenseTypes = new List<string> { "Single Seat License", "Site License", "Concurrent License" };
			return licenseTypes;
		}
	}
}