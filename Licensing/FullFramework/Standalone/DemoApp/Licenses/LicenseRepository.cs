using Sp.Agent;
using System.Collections.Generic;
using System.Linq;

namespace DemoApp.Licenses
{
	public class LicenseRepository
	{
		public static int LicenseCount( IProductContext productContext )
		{
			return productContext.Licenses.All().Count();
		}

		public static IEnumerable<LicenseItemModel> RetrieveAllLicenses( IProductContext productContext )
		{
			return
				from license in productContext.Licenses.All()
				select new LicenseItemModel()
				{
					ActivationKey = license.ActivationKey,
					ValidUntil = license.ValidUntil,
					Features = license.Advanced.AllFeatures().Select( f => f.Key )
				};
		}
	}
}