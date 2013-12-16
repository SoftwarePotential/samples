using System.Web.Mvc;
using System.Web.Routing;
using Sp.Agent.Execution;

namespace Sp.Samples.Agent.Mvc.Controllers
{
	public class LicensedFeaturesController : Controller
	{
		//
		// GET: /LicensedResources/
		public ActionResult Index()
		{
			return View();
		}

		//
		// GET: /LicensedResources/Feature1
		[NotLicensedRedirectToCustomPage]
		[Demoapp_10.Features.Feature1]
		public ActionResult Feature1()
		{
			return ViewFeature( "Feature1" );
		}

		//
		// GET: /LicensedResources/Feature2
		[NotLicensedRedirectToCustomPage]
		[Demoapp_10.Features.Feature2]
		public ActionResult Feature2()
		{
			return ViewFeature( "Feature2" );
		}

		//
		// GET: /LicensedResources/Feature3
		[NotLicensedRedirectToCustomPage]
		[Demoapp_10.Features.Feature3]
		public ActionResult Feature3()
		{
			return ViewFeature( "Feature3" );
		}

		ActionResult ViewFeature( string featureName )
		{
			ViewData[ "Feature" ] = featureName;
			return View( "Feature" );
		}

		public ActionResult NotLicensed()
		{
			return View();
		}
	}

	class NotLicensedRedirectToCustomPageAttribute : NotLicensedRedirectAttribute
	{
		public NotLicensedRedirectToCustomPageAttribute()
			: base( "LicensedFeatures", "NotLicensed" )
		{
		}
	}

	class NotLicensedRedirectAttribute : HandleErrorAttribute
	{
		public string Controller { get; private set; }
		public string Action { get; private set; }

		public NotLicensedRedirectAttribute( string controller, string action )
		{
			Controller = controller;
			Action = action;
			base.ExceptionType = typeof( NotLicensedException );
		}

		public override void OnException( ExceptionContext filterContext )
		{
			if ( filterContext.ExceptionHandled || !ExceptionType.IsInstanceOfType( filterContext.Exception ) )
				return;

			var redirectTarget = new RouteValueDictionary
			{
				{"action", Action},
				{"controller", Controller}
			};

			filterContext.Result = new RedirectToRouteResult( redirectTarget );

			filterContext.ExceptionHandled = true;
			filterContext.HttpContext.Response.Clear();
		}
	}
}