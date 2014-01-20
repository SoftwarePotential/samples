using DemoApp.Activation;
using DemoApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Web.Mvc;

namespace DemoApp.Controllers
{
	public class HomeController : Controller
	{

		readonly string _fileExtension = ".bin"; // Convention for license files is to use a .bin extension
		readonly string _activationUrl = "http://srv.softwarepotential.com/SLMServerWS/ActivationWS.svc";

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

			if ( license != null )
				manualActivationModel.FileId = SaveLicenseFile( license );

			return View( manualActivationModel );
		}

		public FilePathResult Download( Guid fileId )
		{
			string fileName = fileId + _fileExtension;
			var filePath = Path.Combine( TempFolderBasePath, fileName );
			return new FilePathResult( filePath, "application/octet-stream" ) { FileDownloadName = fileName };
		}

		byte[] GetLicense( string activationRequest )
		{
			byte[] requestBlob = ActivationRequestHelper.ExtractRequestBlob( activationRequest );
			return SendActivationRequest( requestBlob );
		}

		byte[] SendActivationRequest( byte[] requestBlob )
		{
			var client = new ActivationWSClient( new BasicHttpBinding(), new EndpointAddress( _activationUrl ) );
			try
			{
				return client.ActivateLicense( requestBlob, "Manual activation" );
			}
			catch ( FaultException ex )
			{
				var translatedExceptionMessage = ActivationExceptionHelper.TranslateExceptionToMessage( ex );
				ModelState.AddModelError( string.Empty, translatedExceptionMessage );
				return null;
			}
		}

		Guid SaveLicenseFile( byte[] license )
		{
			var fileId = Guid.NewGuid();
			string fullPath = Path.Combine( TempFolderBasePath, fileId + _fileExtension );
			System.IO.File.WriteAllBytes( fullPath, license );
			return fileId;
		}

		string TempFolderBasePath
		{
			get { return AppDomain.CurrentDomain.GetData( "DataDirectory" ).ToString(); }
		}
	}
}