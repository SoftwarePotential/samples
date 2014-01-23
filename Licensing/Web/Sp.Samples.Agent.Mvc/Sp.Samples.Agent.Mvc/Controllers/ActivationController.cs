using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Sp.Agent;
using Sp.Samples.Agent.Mvc.Models;
using Resource = Sp.Samples.Agent.Mvc.App_LocalResources;

namespace Sp.Samples.Agent.Mvc.Controllers
{
#if MVC4TAP 
	// The following code requires
	// a) MVC 4 or later
	// b) C# async keyword support, which requires both:
	//	  - VS2012/the C# 5.0 compiler 
	//    - either target .NET 4.5 to get native support or target .NET 4.0 and add the AsyncTargetingPack‎ NuGetPackage
	public class ActivationController : Controller
	{
		//
		// POST: /Activation/Add
		[ValidateAntiForgeryToken]
		[HttpPost]
		public async Task<ActionResult> Add( ActivationModel model )
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
				await SpAgent.Product.Activation.OnlineActivateAsync( model.ActivationKey );
			}
			catch ( Exception ex )
			{
				ModelState.AddModelError( "", string.Format( Resource.Controllers.ActivationController.FailureMessage, model.ActivationKey, ex.Message ) );
				// Re-render the page (with a new PostToken so that a form resubmission will pass the PostToken validation)
				return View( new ActivationModel { PostToken = Guid.NewGuid(), ActivationKey = model.ActivationKey } );
			}

			// Redirect as per PRG to prevent resubmits
			return RedirectToAction( "Success" );
		}
#else
	// The following code has a low bar in terms of dependencies:-
	// a) VS2010 or later
	// b) MVC 3 or later
	// Yes, it really is the idiomatic way to have an Action call an Async Task as part of its activities. 
	// If at all possible, we recommend using the version above
	public class ActivationController : AsyncController
	{
		#region MVC3 pre Task Async support translation of code above, please do all you can to use the C# async version instead
		//
		// POST: /Activation/Add
		[ValidateAntiForgeryToken]
		[HttpPost]
		public void AddAsync( ActivationModel model )
		{
			//if ( !ModelState.IsValid )
			//	return View( model );

			//if ( PostTokenIsMostRecentlySubmittedOne( model.PostToken ) )
			//{
			//	// Resubmits get bounced with an error message (only a refresh will generate a new token)- client side JS should prevent this from being a normal occurrence
			//	ModelState.AddModelError( "", Resource.Controllers.ActivationController.AlreadySubmittedErrorMessage );
			//	return View( model );
			//}

			// Prevent any resubmits (minus ones just for validation purposes) as that could result in another activation taking place from Software Potential service's perspective
			StashLastRequestToken( model.PostToken );

			AsyncManager.Parameters[ "activationKey" ] = model.ActivationKey;
			AsyncManager.OutstandingOperations.Increment();
			AsyncManager.Parameters[ "task" ] =
				SpAgent.Product.Activation.OnlineActivateAsync( model.ActivationKey )
				.ContinueWith( task =>
				{
					AsyncManager.OutstandingOperations.Decrement();
					if ( task.IsFaulted )
						throw task.Exception;
				} );
		}

		public ActionResult AddCompleted( Task task, string activationKey )
		{
			try
			{
				task.Wait();
			}
			catch ( Exception ex )
			{
				string exceptionMessage = ex is AggregateException ? ex.InnerException.Message : ex.Message;
				ModelState.AddModelError( "", string.Format( Resource.Controllers.ActivationController.FailureMessage, activationKey, exceptionMessage ) );
				// Re-render the page (with a new PostToken so that a form resubmission will pass the PostToken validation)
				return View( new ActivationModel { PostToken = Guid.NewGuid(), ActivationKey = activationKey } );
			}

			// Redirect as per PRG to prevent resubmits
			return RedirectToAction( "Success" );
		}
		#endregion
#endif

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