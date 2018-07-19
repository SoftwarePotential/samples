# .NET Core Standalone Console Application

This solution contains a simple project illustrating Software Potential licensing in a .NET Core console app. 

## Prerequisites
* .NET Core 2.0 or later
* Visual Studio Code / Visual Studio 2017 15.7 or later

# Configuring The Sample
## Create the product

* Navigate to the [Software Potential service product page](https://srv.softwarepotential.com/Products.aspx)
* Create a product entitled `Demo` with version `1.0` (case-sensitive) in a non-production account
  * Add two Features (`Feature1`,`Feature2`)

## Install the Software Potential NuGet packages
 This sample requires the following packages to be installed using the authenticated NuGet feed at `https://srv.softwarepotential.com/NuGet/nuget/`:
 * `SoftwarePotential.Protection-<PermutationShortCode>` 
 * `SoftwarePotential.Configuration.Local.SingleUser-<PermutationShortCode>`
 * `SoftwarePotential.Licensing-Demo_10`

 If you are not using the Visual Studio package manager, add these to the PackageReference ItemGroup in the ConsoleApp.csproj and set the software potential feed and your user credintials in a NuGet.Config file (see [here](https://docs.microsoft.com/en-us/nuget/consume-packages/configuring-nuget-behavior) for more guidance). These can then be read automatically when restoring packages for publishing using the dotnet CLI. 

## Dependencies
The application uses the [ManyConsole.CommandLineUtils NuGet pakcage](https://www.nuget.org/packages/ManyConsole.CommandLineUtils).


# Using The Sample
Restore NuGet packages, build the sample and publish, e.g. by running `dotnet publish <path/to/solution/folder>` on the command line. 

You can run the application using ```dotnet <path/to/publish/folder>ConsoleApp.dll```.

## Activating a license

* Generate a license for the Product `Demo` and Version `1.0` via the [Software Potential service](https://srv.softwarepotential.com/Issue.aspx?IssueType=new) 
 * Create a license for product `Demo 1.0` (**Manage Licenses|Issue New Licenses** on the site)
  * Add a Feature (e.g., `Feature1`)

### Online
Run ```dotnet <path/to/publish/folder>ConsoleApp.dll activate -k <yourActivationKey>```

### Offline
* Run ```dotnet <path/to/publish/folder>ConsoleApp.dll generate -k <yourActivationKey>```to process an activation request automatically and save it to disk. This can then be processed via the [Software Potential Manual Activation page](http://manualactivation.softwarepotential.com).
* On a successful activation a License (.bin) file will be generated and made available for download.

  * EITHER Copy the downloaded file on your desktop, and run * Run ```dotnet <path/to/publish/folder>ConsoleApp.dll install -f <theLicenseFileName>```.

  * OR Run ```dotnet <path/to/publish/folder>ConsoleApp.dll install -f <theLicenseFileName> -d <thePathToTheDirectoryContainingTheLicenseFile>```.

## Viewing the licenses list
Run ```dotnet <path/to/publish/folder>ConsoleApp.dll list```.

## Delete an activated License
Run ```dotnet <path/to/publish/folder>ConsoleApp.dll delete -k <yourActivationKey>```.

## Running protected code
* Run ```dotnet <path/to/publish/folder>ConsoleApp.dll Feature1``` - this should display a success message.
* Run ```dotnet <path/to/publish/folder>ConsoleApp.dll Feature2``` - this should display a denial message.

## Help Menu
Run ```dotnet <path/to/publish/folder>ConsoleApp.dll -h```

# Troubleshooting

The most common problems reported with the sample are:

## Sample won't compile

* Ensure you have added the following NuGet packages to the `DemoApp` project:
 * `SoftwarePotential.Protection-<PermutationShortCode>`
 * `SoftwarePotential.Configuration.Local.SingleUser-<PermutationShortCode>` 
 * `SoftwarePotential.Licensing-Demo_10`

## License will not activate

* Ensure _Locking_ is set to **Machine** (the `Sp.Agent` runtime does not support other binding styles)

## Packages Not Displaying in NuGet feed

* Your permutation may be too old to have packages associated with it.
 * Update your permutation on the [Software Potential service's permutations section in the account page](https://srv.softwarepotential.com/Permutations.aspx)
* You have not created the `Demo` product 

## Documentation

Please see below for a list of recommended reading, ranging from Getting Started guides to more detailed technical documentation:

* [Getting Started - Licensing with Software Potential](https://support.softwarepotential.com/hc/en-us/articles/115001354529-Getting-Started-Licensing-with-Software-Potential)
* [Licensing-README](https://support.softwarepotential.com/hc/en-us/articles/115001358849-Licensing-README)
* [SoftwarePotential.Configuration.Local.SingleUser-README](https://support.softwarepotential.com/hc/en-us/articles/115001365849--SingleUser-Configuration-README)
* [Software Potential NuGet Feed](https://support.softwarepotential.com/hc/en-us/articles/115001371425-Getting-Started-Software-Potential-NuGet-Feed)
* [Software Potential APIs](http://api.softwarepotential.com/index.html)

## Support
If you run into any issues visit the [Software Potential support page](https://support.softwarepotential.com) for more detailed information or to submit a help request.

Good luck!
