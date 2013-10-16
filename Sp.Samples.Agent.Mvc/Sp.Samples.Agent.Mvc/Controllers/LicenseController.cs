using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Sp.Agent;
using Sp.Agent.Licensing;
using Sp.Samples.Agent.Mvc.Models;

namespace Sp.Samples.Agent.Mvc.Controllers
{
	public class LicenseController : Controller
	{
		//
		// GET: /License/
		public ActionResult Index()
		{
			var licenses = SpAgent.Product.Licenses.All();

			return View( licenses.ToModel().ToArray() );
		}

		public ActionResult Delete( string activationKey )
		{
			SpAgent.Product.Stores.Delete( activationKey );

			return RedirectToAction( "Index" );
		}

	}

	static class LicenseModelExtensions
	{
		public static IEnumerable<LicenseModel> ToModel( this IEnumerable<ILicense> thats )
		{
			return
				from license in thats
				select new LicenseModel
				{
					ActivationKey = license.ActivationKey,
					ExpirationDate = license.Advanced.Period.EndDate,
					Features = string.Join( ", ", from feature in license.Advanced.AllFeatures() select feature.Key )
				};
		}
	}
}