using System;
using System.Web.Mvc;
using Sp.Agent;
using Sp.Samples.Agent.Mvc.Models;
using Resource = Sp.Samples.Agent.Mvc.App_LocalResources;

namespace Sp.Samples.Agent.Mvc.Controllers
{
	public class ActivationController : Controller
	{
		//
		// GET: /Activation/
		public ActionResult Index()
		{
			return RedirectToAction( "Add" );
		}

		//
		// GET: /Activation/Add
		public ActionResult Add()
		{
			return View( new ActivationModel { PostToken = Guid.NewGuid() } );
		}

		//
		// POST: /Activation/Add
		[ValidateAntiForgeryToken]
		[HttpPost]
		public ActionResult Add( ActivationModel model )
		{
			if ( !ModelState.IsValid )
				return View( model );

			if ( PostTokenIsMostRecentlySubmittedOne( model.PostToken ) )
			{
				// Resubmits get bounced with an error message (only a refresh will generate a new token)- client side JS should prevent this from being a normal occurrence
				ModelState.AddModelError( "", Resource.Controllers.ActivationController.AlreadySubmittedErrorMessage );
				return View( model );
			}

			// Prevent any resubmits (minus ones just for validation purposes) as that could result in another activation taking place from Software Potential service's perspective
			StashLastRequestToken( model.PostToken );

			try
			{
#if ONLINE_ACTIVATE_WORKS_CONSISTENTLY
				SpAgent.Product.Activation.OnlineActivate( model.ActivationKey );
#else
				//TODO - switch back to OnlineActivate() once it's proven to work consistently in all environments
				SpAgent.Product.Activation.OnlineActivateAsync( model.ActivationKey ).Wait();
#endif
			}
			catch ( Exception ex )
			{
				string exceptionMessage = ex is AggregateException ? ex.InnerException.Message : ex.Message;
				ModelState.AddModelError( "", string.Format( Resource.Controllers.ActivationController.FailureMessage, model.ActivationKey, exceptionMessage ) );
				// Re-render the page (with a new PostToken so that a form resubmission will pass the PostToken validation)
				return View( new ActivationModel { PostToken = Guid.NewGuid(), ActivationKey = model.ActivationKey } );
			}

			// Redirect as per PRG to prevent resubmits
			return RedirectToAction( "Success" );
		}

		//
		// GET: /Activation/Success
		public ActionResult Success()
		{
			return View();
		}

		void StashLastRequestToken( Guid token )
		{
			Session[ "LASTTOKEN" ] = token;
		}

		bool PostTokenIsMostRecentlySubmittedOne( Guid token )
		{
			var lastToken = Session[ "LASTTOKEN" ] as Guid?;
			return lastToken == token;
		}
	}
}