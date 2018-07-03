The MVC sample (see [`Sp.Samples.Agent.Mvc`](https://github.com/SoftwarePotential/samples/tree/master/Sp.Samples.Agent.Mvc)) illustrates key aspects of licensing an ASP .NET MVC 3 web application.

## What the sample is intended to demonstrate
* `Sp.Agent` configuration using the Software Potential NuGet packages
* declarative licensing / protection of functionality via attributes
* handling online activation (accepting/validating a key and using that to install a license)
* showing a list of installed licenses and their status
* using WebDeploy / Visual Studio's Publish Web dialog to deploy your licensed solution correctly

## Prerequisites
* .NET 4.0 or later
* Visual Studio 2010 or later
* [NuGet Package Manager](http://visualstudiogallery.msdn.microsoft.com/27077b70-9dad-4c64-adcf-c7cf6bc9970c) 2.5 or later
* A working [Microsoft Web Deploy](http://www.iis.net/downloads/microsoft/web-deploy) installation (if you want to test the deployment facility)

## Configuring The Sample
### Create the product

* Navigate to the [Software Potential service product page](https://srv.softwarepotential.com/Products.aspx)
* Create a product entitled `Demo` with version `1.0` (case-sensitive) in a non-production account
  * Add two Features (`Feature1`,`Feature2`)

### Install the NuGet packages
* Add the SoftwarePotential NuGet feed (`https://srv.softwarepotential.com/NuGet/nuget/`) in your Package Manager settings
* Add the following package references:
 * `SoftwarePotential.Protection-<PermutationShortCode>` 
 * `SoftwarePotential.Configuration.Web-<PermutationShortCode>`
 * `SoftwarePotential.Licensing-Demo_10`

## Building and running the sample
The sample can be built using Visual Studio and can be hosted either in IIS, or in the built-in VS development server.

## Deploying to IIS
The application is intended to be deployed to an IIS Web Server using WebDeploy. The simplest way to do that is to use Publish Web wizard in Visual Studio.

**NB the application stores licenses under `<application base directory>\App_Data\Licenses`. When (re)deploying, it is critical to ensure that the publish process does not delete any licenses that users may have activated**. To do this:
* In Visual Studio 2010: Ensure the *`Leave extra files on destination (do not delete)`* option (in Publish Web dialog) is **checked**
* In Visual Studio 2012 or 2013: Ensure the *`Remove additional files at destination`* option (in Publish Web dialog) is **_un_checked**

## Documentation

Please see below for a list of recommended reading, ranging from Getting Started guides to more detailed technical documentation:

* [Getting Started - Licensing with Software Potential](https://support.softwarepotential.com/hc/en-us/articles/115001354529-Getting-Started-Licensing-with-Software-Potential)
* [Licensing README](https://support.softwarepotential.com/hc/en-us/articles/115001358849-Licensing-README)
* [Web Configuration README](https://support.softwarepotential.com/hc/en-us/articles/115001366649-Web-Configuration-README)
* [Software Potential NuGet Feed](https://support.softwarepotential.com/hc/en-us/articles/115001371425-Getting-Started-Software-Potential-NuGet-Feed)
* [Software Potential APIs](http://api.softwarepotential.com/index.html)

## Support
If you run into any issues visit the [Software Potential support page](https://support.softwarepotential.com) for more detailed information or to submit a help request.

Good luck!
