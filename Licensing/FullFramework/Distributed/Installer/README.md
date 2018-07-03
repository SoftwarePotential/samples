##Using the Sample

### Customize
* Change `DistributorVendor` variable in `Variables.wxi` to your company name (`DistributorVendor` combined with `DistributorProductName` compose the Program Name in the Add/Remove Programs list; by default, the full Program Name is _My Company Software Potential Distributor_)

### Distributor Server installation and uninstallation
* A successful build of the Installer project generates a `DistributorSetup.msi` file in the output directory (`bin\$(Configuration)`).
* Distributor Server can be installed by double-clicking the `DistributorSetup.msi` file in the Windows Explorer. Alternatively, `msiexec /i DistributorSetup.msi` can be used from the command line.
    - Distributor Server will be registered as a Windows Service (_Software Potential Distributor_)
    - Also, the full Program Name will appear in Programs and Features (or Add/Remove Programs) list. By default, the full Program Name is _My Company Software Potential Distributor_ (see _Configuring the sample_ section for the information how to customize it)
* Distributor Server can be uninstalled from the Add/Remove Programs (Programs and Features) list in the Control Panel. Alternatively, `msiexec /x DistributorSetup.msi` can be used from the command line.

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


