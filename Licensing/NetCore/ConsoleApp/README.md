# .NET Core Standalone Console Application

This solution contains a simple project illustrating Software Potential licensing in a .NET Core console app. 

## Prerequisites
* [.NET Core 2.0](https://www.microsoft.com/net/download) or later.
*  A code editor (e.g. [Visual Studio Code](https://code.visualstudio.com/) or [Atom](https://atom.io/)) or IDE (eg. [Visual Studio 15.7](https://docs.microsoft.com/en-us/visualstudio/install/update-visual-studio) or later).

## Dependencies
The application uses the [ManyConsole.CommandLineUtils NuGet package](https://www.nuget.org/packages/ManyConsole.CommandLineUtils).

## Configuring the sample

### Create a Software Potential Product
  * Navigate to the Software Potential service [Define > Create Products page](https://srv.softwarepotential.com/Products.aspx)
  * Create a Product entitled `Demo` with version `1.0` (case-sensitive) in a non-production account
  * Add two Features named `Feature1` and `Feature2`.

These Product and Feature definitions correspond to selected functionality in the sample source code, and licensing is implemented in the code with the use of declarative Software Potential Protection Attributes. Functions decorated with these attributes will only execute if a valid license which includes a particular defined Feature has been activated (see [Getting Started - Licensing with Software Potential](https://support.softwarepotential.com/hc/en-us/articles/115001354529-Getting-Started-Licensing-with-Software-Potential) for a more detailed guide to Declarative Licensing).

In the sample, the functions which execute functionality corresponding to Features defined in the service (i.e. `Feature1` and `Feature2`) are called `ExecuteFeature1` and `ExecuteFeature2`, and are located in `ExecuteFeatures.cs`.

To use the sample with a product that you may have already defined in the Software Potential service, see **Using the sample with a pre-existing Product** below.

## Install the Software Potential NuGet packages for the .NET solution
* Ensure that your permutation version is 4.0.2003 or later on the Software Potential service [Develop > Manage Permutations page](https://srv.softwarepotential.com/Permutations.aspx).
    *  To update, select the permutation and click **Update**.
* Add the SoftwarePotential NuGet feed (`https://srv.softwarepotential.com/NuGet/nuget/`) to your NuGet configuration.
* Add the following package references to the ImageEditor.Core project:
  * `SoftwarePotential.Protection-<MyPermutationShortCode>` 
  * `SoftwarePotential.Configuration.Local.SingleUser-<MyPermutationShortCode>`
  * `SoftwarePotential.Licensing-<MyProduct_Version>` (if you are using a newly created trial product as specified above, this would be `SoftwarePotential.Licensing-Demo_10`)
 
See [Getting Started - Software Potential NuGet Feed](https://support.softwarepotential.com/hc/en-us/articles/115001371425-Getting-Started-Software-Potential-NuGet-Feed)  or [NuGet Package Management Without the Visual Studio Package Manager](https://support.softwarepotential.com/hc/en-us/articles/360007928993) for more detailed guidance on configuring the Software Potential NuGet feed and adding packages.


## Using The Sample
Restore NuGet packages, build the sample and publish, e.g. by running 
```
  dotnet publish <path\to\solution\folder>
```
on the command line. This will publish any files necessary to run the sample to `bin\Debug\<framework>\publish`  

You can run the application using 
```
  dotnet <path\to\publish\folder>\ConsoleApp.dll
```

To publish the sample as a [self contained deployment](https://docs.microsoft.com/en-us/dotnet/core/deploying/index#self-contained-deployments-scd) which can be run on machines that do not have the .Net Core framework installed, you should add the `-r|--runtime` option to the `dotnet publish` command, followed by the Runtime Identifier for the targeted runtime, E.G:
```
  dotnet publish <path\to\solution\folder> -r win10-x64 -c Release
```
In this case, the publish folder will be located at `bin\Debug\<framework>\win10-x64\publish` and contain an executable which can be run from the command line:
```
   <path\to\publish\folder>\ConsoleApp.exe
```
or, if you are targeting Mac OS or Linux:
```
   <path/to/publish/folder>/ConsoleApp
```

If you are running the published sample on Linux you may have to change the permissions on the file to run it:
```
   chmod 777 <path/to/publish/folder>/ConsoleApp
```

For a list of Runtime Identifiers (RID's), see the [RID catalog](https://docs.microsoft.com/en-us/dotnet/core/rid-catalog).
For .Net Core CLI reference see [the Microsoft documentation](https://docs.microsoft.com/en-us/dotnet/core/tools).

### Activating a license

* Generate a license for the Product `Demo` and Version `1.0` via the [Software Potential service](https://srv.softwarepotential.com/Issue.aspx?IssueType=new) 
 * Create a license for product `Demo 1.0` (**Manage Licenses|Issue New Licenses** on the site)
  * Add a Feature (e.g., `Feature1`)

#### Online
Run 
```
  dotnet <path\to\publish\folder>\ConsoleApp.dll activate -k <yourActivationKey>
```

#### Offline
* Run 
```
  dotnet <path\to\publish\folder>\ConsoleApp.dll generate -k <yourActivationKey>
```
  to process an activation request automatically and save it to disk. This can then be processed via the [Software Potential Manual Activation page](http://manualactivation.softwarepotential.com).
* On a successful activation a License (.bin) file will be generated and made available for download.

  * EITHER Copy the downloaded file on your desktop, and run
```
  dotnet <path\to\publish\folder>\ConsoleApp.dll install -f <theLicenseFileName>
```

  * OR Run
   ```
    dotnet <path\to\publish\folder>\ConsoleApp.dll install -f <theLicenseFileName> -d <path\to\folder\containing\the\licenseFile>
  ```

### Viewing the licenses list
Run 
```
  dotnet <path\to\publish\folder>\ConsoleApp.dll list
```

### Delete an activated License
Run
```
  dotnet <path\to\publish\folder>\ConsoleApp.dll delete -k <yourActivationKey>
```

### Running protected code
Run
```
  dotnet <path\to\publish\folder>\ConsoleApp.dll Feature1
```
```
  dotnet <path\to\publish\folder>\ConsoleApp.dll Feature2
```

### Help Menu
Run 
```
  dotnet <path\to\publish\folder>\ConsoleApp.dll -h
```

## Using the sample with a pre-existing Product
Out of the box, the sample can be used with a Product called `Demo` version `1.0` with Features named `Feature1` and `Feature2`. However, if you opt to use an existing product you will need to edit the Software Potential Protection Attributes in the .Net source code:
  * Open `ExecuteFeatures.cs`.
  * Replace attributes referencing `Demo_10.Features.Feature1` and `Demo_10.Features.Feature2` with your selected product and features e.g.
  ```
  ...
  [MyExistingProduct_Version.Features.MyFeature]
  public void ExecuteFeature1()
		{
  ...
  ```

## Troubleshooting

The most common problems reported with the sample are:

### Sample won't compile

* Ensure you have added the following NuGet packages to the `DemoApp` project:
 * `SoftwarePotential.Protection-<PermutationShortCode>`
 * `SoftwarePotential.Configuration.Local.SingleUser-<PermutationShortCode>` 
 * `SoftwarePotential.Licensing-Demo_10`

### License will not activate

* Ensure _Locking_ is set to **Machine** (the `Sp.Agent` runtime does not support other binding styles)

### Packages Not Displaying in NuGet feed

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
* [Microsoft .Net Core reference](https://docs.microsoft.com/en-us/dotnet/core/);
## Support
If you run into any issues visit the [Software Potential support page](https://support.softwarepotential.com) for more detailed information or to submit a help request.

Good luck!
