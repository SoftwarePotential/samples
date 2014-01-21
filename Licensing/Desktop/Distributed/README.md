# Distributor Sample
This solution contains projects illustrating the following elements of a Distributor-aware **Licensed** application in a **Desktop** environment:

* Obtaining licenses from a local computer or from the network (using Distributor service)
* Configuring licensing settings (including Distributor connection settings)
* Managing local licenses (Online Activation dialog, License List dialog)
* Creating a Windows Installer MSI package for Distributor Server (using WiX toolset)

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
* Add the following packages to your solution:
	- `Sp.Distributor-<PermutationShortCode>` (needed to build the Installer project)

### Customize the Installer project
* Change `DistributorVendor` variable in `Variables.wxi` to your company name (`DistributorVendor` combined with `DistributorProductName` compose the Program Name in the Add/Remove Programs list; by default, the full Program Name is _My Company Software Potential Distributor_)

##Using the Sample

### Distributor Server installation and uninstallation
* A successful build of the Installer project generates a `DistributorSetup.msi` file in the output directory (`bin\$(Configuration)`).
* Distributor Server can be installed by double-clicking the `DistributorSetup.msi` file in the Windows Explorer. Alternatively, `msiexec /i DistributorSetup.msi` can be used from the command line.
    - Distributor Server will be registered as a Windows Service (_Software Potential Distributor_)
    - Also, the full Program Name will appear in Programs and Features (or Add/Remove Programs) list. By default, the full Program Name is _My Company Software Potential Distributor_ (see _Configuring the sample_ section for the information how to customize it)
* Distributor Server can be uninstalled from the Add/Remove Programs (Programs and Features) list in the Control Panel. Alternatively, `msiexec /x DistributorSetup.msi` can be used from the command line.

### Configure Licensing
* Run `DemoApp` project.
* Enter Distributor URL in the Licensing Configuration dialog, which will be displayed on the first application run
	-	for a locally installed Distributor Server, the default URL is `http://localhost:8731`
	-	click `Save`
* The Licensing Configuration dialog can always be accessed from **Licensing|Configure** on the sample's menu

### Issue and install a network license
* Generate a Distributor license for the Product `Demo` and Version `1.0` via the [Software Potential service](https://srv.softwarepotential.com/Issue.aspx?IssueType=new) 
   * Add a Feature (e.g., `Feature1`)
   * Check **For use with Distributor only** checkbox
   * See _Getting Started With Distributor_ for information how to install the license in the Distributor Server

### Activating a local license
* Generate a license for the Product `Demo` and Version `1.0` via the [Software Potential service](https://srv.softwarepotential.com/Issue.aspx?IssueType=new) 
  * Add a Feature (e.g., `Feature2`)
* Paste the license key into the activation form (open **Licensing|Configure** on the sample's menu, then click **Activate**)

### Running protected code
* Click the **Run Feature 1** button - this should display a success message (we have a license for this feature in the Distributor Server)
* Click the **Run Feature 2** button - this should display a success message (we have a license for this feature installed locally)
* Click the **Run Feature 3** button - this should display a denial message and navigate you to the activation dialog

### Upgrading to a newer Distributor Server version
* Upgrade your `Sp.Distributor-<PermutationShortCode>` NuGet package to a newer version in the Package Manager
* Build the Installer project
* A new version of `DistributorSetup.msi` is generated in the output directory
* Installing the new version of `DistributorSetup.msi` will perform a complete upgrade of Distributor Server on the target machine

#Troubleshooting
## The Installer project fails to load in Visual Studio
* Please make sure that the [WiX toolset](http://wixtoolset.org) is installed (including the Visual Studio extension)

## You get a _Could not find a Software Potential Distributor Server_ error message when you build the Installer project
* Please either:
 1. add the `Sp.Distributor-<PermutationShortCode>` package to your solution to have it auto-discovered.
 2. OR override the path by assigning a valid directory path to `$(DistributorSourceDirectory)` in the .props file.
* If you have installed `Sp.Distributor-<PermutationShortCode>` package but the problem persists, check where NuGet stores packages for your solution. The default location is the `packages` folder in the solution folder. 
    - If the NuGet packages aren't stored in the default location, please open the `Sp.Distributor.Wix.props` file and amend the `NugetRepositoryFolder` property (e.g. `..\..\MyNugetPackagesRepositoryCustomDir`)
    -  Alternatively, please open the `Sp.Distributor.Wix.props` file and set `DistributorSourceDirectory` to the full path to the Distributor Server Host (e.g. `..\packages\Sp.Distributor-abcde\Slps.Distributor.Host`)

## Visual Studio seems to ignore changes made in .targets or .props files
* Please re-open the solution as .targets and .props files may be cached by Visual Studio

