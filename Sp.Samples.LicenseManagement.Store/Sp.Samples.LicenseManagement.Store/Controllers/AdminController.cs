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

using Sp.Samples.LicenseManagement.Store.Models;
using System.Web.Mvc;

namespace Sp.Samples.LicenseManagement.Store.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

		public ActionResult SetCredentials()
		{
			var credentials = new CredentialsModel();
			return View( credentials );
		}

		[HttpPost]
		public ActionResult SetCredentials( CredentialsModel credentials )
		{
			if ( ModelState.IsValid )
			{
				var file = SoftwarePotentialConfiguration.File;
				file.WriteCredentials( credentials.Username, credentials.Password );
				return RedirectToAction( "Index", "Home" );
			}
			return View( credentials );
		}
    }
}
