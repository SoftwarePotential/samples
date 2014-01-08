# Distributor Server Sample Installer

This solution illustrates how to create a Windows Installer MSI package for software Potential Distributor Server, using WiX toolset.

#Prerequisites
* Visual Studio 2010 or later
* [WiX toolset](http://wixtoolset.org) 3.5 or later
* [NuGet Package Manager](http://visualstudiogallery.msdn.microsoft.com/27077b70-9dad-4c64-adcf-c7cf6bc9970c) 2.5 or later

#Configuring the sample
* Install the Software Potential Distributor Server NuGet package
    - Add the SoftwarePotential NuGet feed (`https://srv.softwarepotential.com/NuGet/nuget/`) in your Package Manager settings (see [Software Potential NuGet Feed](http://docs.softwarepotential.com/Adding-SoftwarePotential-NuGet-Feed.html))
    - Add an `Sp.Distributor-<PermutationShortCode>` package to your solution
* Change `DistributorVendor` variable in `Variables.wxi` to your company name

#Troubleshooting
## The Installer project fails to load in Visual Studio
* Make sure that the WiX toolset is installed (including the Visual Studio extension)

## You get a `Could not find/detect a Software Potential Distributor Server` error message
* Please either:
 1. add the `Sp.Distributor-<PermutationShortCode>` package to your solution to have it auto-discovered.
 2. OR override the path by assigning a valid directory path to `$(DistributorSourceDirectory)` in the .props file."

## Visual Studio seems to ignore changes made in .targets or .props files
* Please re-open the solution as .targets and .props files seem to be cached by Visual Studio


