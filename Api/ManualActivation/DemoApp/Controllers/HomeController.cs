using DemoApp.Activation;
using DemoApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Web.Mvc;
using System.ServiceModel;

namespace DemoApp.Controllers
{
	public class HomeController : Controller
	{

		readonly string _fileExtension = ".bin";

		string BaseUrl
		{
			get { return AppDomain.CurrentDomain.GetData( "DataDirectory" ).ToString(); }
		}

		public ActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public ActionResult Index( ManualActivationModel manualActivationModel )
		{
			if ( !ModelState.IsValid )
				return View( manualActivationModel );
			var license = GetLicense( manualActivationModel.ActivationRequest );
			if ( license != default( byte[] ) )
				manualActivationModel.FileId = SaveLicenseFile( license );
			return View( manualActivationModel );
		}

		public FilePathResult Download( Guid fileId )
		{
			var filePath = Path.Combine( BaseUrl, fileId + _fileExtension );
			var result = new FilePathResult( filePath, "application/octet-stream" );
			result.FileDownloadName = fileId + _fileExtension;
			return result;
		}

		byte[] GetLicense( string activationRequest )
		{
			byte[] requestBlob = ActivationRequestHelper.ExtractRequestBlob( activationRequest );
			byte[] license = SendActivationRequest( requestBlob );

			return license;
		}

		byte[] SendActivationRequest( byte[] requestBlob )
		{
			var client = new ActivationWSClient(
							new BasicHttpBinding(),
							new EndpointAddress( AppSettings.ActivationWSUrl )
							);
			var license = default( byte[] );
			try
			{
				license = client.ActivateLicense( requestBlob, "Manual activation" );
			}
			catch ( Exception ex )
			{
				var translatedExceptionMessage = ActivationExceptionHelper.TranslateExceptionToMessage( ex );
				ModelState.AddModelError( string.Empty, translatedExceptionMessage );
			}
			return license;
		}

		Guid SaveLicenseFile( byte[] license )
		{
			var fileId = Guid.NewGuid();
			string fullPath = Path.Combine( BaseUrl, fileId + _fileExtension );
			System.IO.File.WriteAllBytes( fullPath, license );
			return fileId;
		}
	}
}