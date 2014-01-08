# Distributor Server Installer Sample

This solution illustrates how to create a Windows Installer MSI package for Software Potential Distributor Server, using WiX toolset.

#Prerequisites
* Visual Studio 2010 or later
* [WiX toolset](http://wixtoolset.org) 3.5 or later
* [NuGet Package Manager](http://visualstudiogallery.msdn.microsoft.com/27077b70-9dad-4c64-adcf-c7cf6bc9970c) 2.5 or later

#Configuring the sample
* Install the Software Potential Distributor Server NuGet package
    - Add the SoftwarePotential NuGet feed (`https://srv.softwarepotential.com/NuGet/nuget/`) in your Package Manager settings (see [Software Potential NuGet Feed](http://docs.softwarepotential.com/Adding-SoftwarePotential-NuGet-Feed.html) for reference)
    - Add an `Sp.Distributor-<PermutationShortCode>` package to your solution
* Change `DistributorVendor` variable in `Variables.wxi` to your company name (`DistributorVendor` combined with `DistributorProductName` form the Program Name in the Add/Remove Programs list; by default, the full Program Name is _My Company Software Potential Distributor_)

#Building and installing the sample
* A successful build of the Installer project yields a `DistributorSetup.msi` file in the output
  
##Installation and uninstallation
* Distributor Server can be installed by double-clicking the `DistributorSetup.msi` file in the Windows Explorer. Alternatively, `msiexec /i DistributorSetup.msi` can be used from the command line.
    - After installation, there full Program Name will appear in Windows Add/Remove Programs (or Programs and Features) list (by default, the full Program Name is _My Company Software Potential Distributor_ - see _Configuring the sample_ section for information how to customize it )
    - Also, Distributor Server will be registered as a Windows Service (_Software Potential Distributor_)
* Distributor Server can be uninstalled from the Add/Remove Programs (Programs and Features) list in the Control Panel. Alternatively, `msiexec /x DistributorSetup.msi` can be used from the command line.

#Troubleshooting
## The Installer project fails to load in Visual Studio
* Make sure that the WiX toolset is installed (including the Visual Studio extension)

## You get a _Could not find/detect a Software Potential Distributor Server..._ error message
* Please either:
 1. add the `Sp.Distributor-<PermutationShortCode>` package to your solution to have it auto-discovered.
 2. OR override the path by assigning a valid directory path to `$(DistributorSourceDirectory)` in the .props file.
* If you have installed `Sp.Distributor-<PermutationShortCode>` package but the problem persists, check where NuGet stores packages for your solution. The default location is in `packages` folder in the solution folder. If that location is different, please open the .props file and set the `NugetRepositoryFolder` property. Alternatively, set `DistributorSourceDirectory` to the full path to the Distributor Server Host. 

## Visual Studio seems to ignore changes made in .targets or .props files
* Please re-open the solution as .targets and .props files may be cached by Visual Studio

