# Distributor Sample
This solution contains projects illustrating the following elements of a Distributor-aware **Licensed** application in a **Desktop** environment:

* Obtaining licenses from a local computer or from the network (using Distributor service)
* Configuring licensing settings (including Distributor connection settings)
* Managing local licenses (Online Activation dialog, License List dialog)
* Managing checkouts
* Creating a Windows Installer MSI package for Distributor Server (using WiX toolset)
* Creating a standalone diagnostics tool for your Distributor Server 

## Prerequisites
* Visual Studio 2010 or later
* [NuGet Package Manager](http://visualstudiogallery.msdn.microsoft.com/27077b70-9dad-4c64-adcf-c7cf6bc9970c) 2.5 or later
* [WiX toolset](http://wixtoolset.org) 3.5 or later (for building the Installer)

## Configuring the sample
### Create the product

* Navigate to the [Software Potential service product page](https://srv.softwarepotential.com/Products.aspx)
* Create a product entitled `Demo` with version `1.0` (case-sensitive) in a non-production account
  * Add two Features (`Feature1`,`Feature2`)

### Install the NuGet packages
* Add the SoftwarePotential NuGet feed (`https://srv.softwarepotential.com/NuGet/nuget/`) in your Package Manager settings (see [Software Potential NuGet Feed](http://docs.softwarepotential.com/Adding-SoftwarePotential-NuGet-Feed.html) for reference)
* Add the following packages to `DemoAp` project:
	-	`SoftwarePotential.Protection-<PermutationShortCode>`
	-	`SoftwarePotential.Licensing-Demo_10`
	-	`SoftwarePotential.Configuration.Local.SingleUser-<PermutationShortCode>`
	-	`SoftwarePotential.Configuration.Distributor-<PermutationShortCode>`
* Add the following packages to `Diagnostics` project:
	-	`Sp.Agent.Distributor-<PermutationShortCode>`
	-	`SoftwarePotential-<PermutationShortCode>`
* Add the following packages to your solution:
	- `Sp.Distributor-<PermutationShortCode>` (needed to build the Installer project)