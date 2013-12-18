The MVC sample (see [`Sp.Samples.Agent.Mvc`](https://github.com/SoftwarePotential/samples/tree/master/Sp.Samples.Agent.Mvc)) illustrates key aspects of licensing an ASP .NET MVC 3 web application.

## What the sample is intended to demonstrate
* `Sp.Agent` configuration using the Software Potential NuGet packages
* declarative licensing / protection of functionality via attributes
* handling online activation (accepting/validating a key and using that to install a license)
* showing a list of installed licenses and their status
* using WebDeploy / Visual Studio's Publish Web dialog to deploy your licensed solution correctly

##Prerequisites
* .NET 4.0 or later
* Visual Studio 2010 or later
* [NuGet Package Manager](http://visualstudiogallery.msdn.microsoft.com/27077b70-9dad-4c64-adcf-c7cf6bc9970c) 2.5 or later
* A working [Microsoft Web Deploy](http://www.iis.net/downloads/microsoft/web-deploy) installation (if you want to test the deployment facility)

##Building and running the sample
The sample comes bundled with all required dependencies and shouldn't need any up-front configuration.

It can be built using Visual Studio and can be hosted either in IIS or using the built-in VS development server.

##Deploying to IIS
The application is intended to be deployed to an IIS Web Server using WebDeploy. The simplest way to do that is to use Publish Web wizard in Visual Studio.

**NB the application stores licenses under `<application base directory>\App_Data\Licenses`. When (re)deploying, it is critical to ensure that the publish process does not delete any licenses that users may have activated**. To do this:
* In Visual Studio 2010: Ensure the *`Leave extra files on destination (do not delete)`* option (in Publish Web dialog) is **checked**
* In Visual Studio 2012 or 2013: Ensure the *`Remove additional files at destination`* option (in Publish Web dialog) is **_un_checked**

##Using your own Software Potential permutation
The sample is configured to use a specific Software Potential permutation, associated with a test account. To protect the application with your own permutation, and license it for your own product, you need to uninstall existing SoftwarePotential NuGet packages and replace them with packages that are available for you from SoftwarePotential NuGet feed.

1. Configure [SoftwarePotential NuGet feed](http://docs.softwarepotential.com/Adding-SoftwarePotential-NuGet-Feed.html) feed in Visual Studio . Make sure that you can authenticate with your Software Potential credentials and see the list of Software Potential packages in Package Manager dialog (in the Online/Software Potential section)
1. Uninstall the following NuGet packages from the sample project (including dependencies):
    * `SoftwarePotential.Licensing-Demoapp_10`
    * `SoftwarePotential.Configuration.Local.SingleUser-62340`
    * `SoftwarePotential.Protection-62340`
1. Install the following packages from Software Potential NuGet feed (use your own permutation short code and product name):
 * `SoftwarePotential.Protection-<permutationShortCode>`
 * `SoftwarePotential.Configuration.Local.SingleUser-<permutationShortCode>`
 * `SoftwarePotential.Licensing-<productNameAndVersionIdentifier>`
1. Delete `SoftwarePotential\SpAgent.Configuration.Local.Customizations.cs` file (the necessary web-specific Sp.Agent configuration customizations are already in `SpAgent.Configuration.Web.Customizations.cs` file)
1. Modify `Controllers\LicenseFeaturesController.cs` to use your own product feature names
