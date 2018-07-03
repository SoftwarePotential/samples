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
* Add the following packages to `DemoApp` project:
	-	`SoftwarePotential.Protection-<PermutationShortCode>`
	-	`SoftwarePotential.Licensing-Demo_10`
	-	`SoftwarePotential.Configuration.Local.SingleUser-<PermutationShortCode>`
	-	`SoftwarePotential.Configuration.Distributor-<PermutationShortCode>`
* Add the following packages to `Diagnostics` project:
	-	`Sp.Agent.Distributor-<PermutationShortCode>`
	-	`SoftwarePotential-<PermutationShortCode>`
* Add the following packages to `Installer` project:
	- `Sp.Distributor-<PermutationShortCode>` 

## Documentation

Please see below for a list of recommended reading, ranging from Getting Started guides to more detailed technical documentation:

* [Getting Started - Licensing with Software Potential](https://support.softwarepotential.com/hc/en-us/articles/115001354529-Getting-Started-Licensing-with-Software-Potential)
* [Getting Started - Distributor](https://support.softwarepotential.com/hc/en-us/articles/115001367189-Getting-Started-Distributor)
* [Licensing README](https://support.softwarepotential.com/hc/en-us/articles/115001358849-Licensing-README)
* [Single User Configuration README](https://support.softwarepotential.com/hc/en-us/articles/115001365849--SingleUser-Configuration-README)
* [Multi User Configuration README](https://support.softwarepotential.com/hc/en-us/articles/115001380105-Multi-User-Configuration-README)
* [Sp.Distributor README](https://support.softwarepotential.com/hc/en-us/articles/115001381945-Sp-Distributor-README)
* [Distributor Client Configuration README](https://support.softwarepotential.com/hc/en-us/articles/115001382105-Distributor-Client-Configuration-README)
* [Software Potential NuGet Feed](https://support.softwarepotential.com/hc/en-us/articles/115001371425-Getting-Started-Software-Potential-NuGet-Feed)
* [Software Potential APIs](http://api.softwarepotential.com/index.html)

## Support
If you run into any issues visit the [Software Potential support page](https://support.softwarepotential.com) for more detailed information or to submit a help request.

Good luck!