# Electron Standalone Desktop Sample application

This solution contains a simple project illustrating the following elements of a **Licensed** Electron desktop application:

* an Online Activation page
* an Offline Activation page
* a license list page
* licensed code execution

In this sample protected features in a .NET assembly are called by Node.js code in an Electron app via the [electron-edge-js](https://github.com/agracio/electron-edge-js) fork of Edge.js.

## Prerequisites 
### All Platforms
* [.NET Core 2.1.300](https://www.microsoft.com/net/download/dotnet-core/2.1) or later. For OS versions supported by .Net Core see [these tables](https://github.com/dotnet/core/blob/master/release-notes/2.1/2.1-supported-os.md).
* (Node.js 8.2.1)(https://nodejs.org/en/) or later
*  A code editor (e.g. [Visual Studio Code](https://code.visualstudio.com/) or [Atom](https://atom.io/)) or IDE (eg. [Visual Studio 15.7](https://docs.microsoft.com/en-us/visualstudio/install/update-visual-studio) or later).

### Ubuntu
* [.Net Core prerequisites](https://docs.microsoft.com/en-us/dotnet/core/linux-prerequisites?tabs=netcore2x)
* [build-essential package](https://packages.ubuntu.com/xenial/build-essential)
* [Mono](https://www.mono-project.com/download/stable/#download-lin) mono-devel development package
* libgtk2.0-dev [GTK](https://www.gtk.org/) development package. 

## Configuring the sample
## Create the product

* Navigate to the [Software Potential service product page](https://srv.softwarepotential.com/Products.aspx)
* Create a product entitled `Demo` with version `1.0` (case-sensitive) in a non-production account
  * Add three Features (`Feature1`, `Feature2` and `Feature3`)

## Install the Software Potential NuGet packages for the .NET solution
* Add the SoftwarePotential NuGet feed (`https://srv.softwarepotential.com/NuGet/nuget/`) in your Package Manager settings
* Add the following package references:
 * `SoftwarePotential.Protection-<PermutationShortCode>` 
 * `SoftwarePotential.Configuration.Local.SingleUser-<PermutationShortCode>`
 * `SoftwarePotential.Licensing-Demo_10`

## Build, publish and package the sample.

### Specifiying the target platform
Our sample targets either Windows 10 64 bit or Ubuntu 18.04 64 bit platforms out of the box.
To specify another target platform:
1. Ascertain the target platform's .Net Core [Runtime Identifier](https://docs.microsoft.com/en-us/dotnet/core/rid-catalog) (RID) by running dotnet --info on the command line.
2. Set the -r argument value to the target platform Runtime Identifier for publish commands specified in `package.json`
```
"scripts": {
    "start": "electron .",
    "publish-win": "npm run clean  && dotnet publish ./src -r <yourTargetWindowsRuntimeIdentifier> -c Release --self-contained -o ../../dotNetAssemblies && rimraf dotNetAssemblies/ImageEditor.Core.deps.json",
    "publish-linux": "npm run clean && dotnet publish ./src -r <yourTargetLinuxRuntimeIdentifier> -c Release --self-contained -o ../../dotNetAssemblies",
    ...
```

AND
3.  Ensure the target platform Runtime Identifier is included in the RuntimeIdentifiers property in `\src\ImageEditor.Core\ImageEditor.Core.csproj`.
```
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>netcoreapp2.1</TargetFramework>
    <PreserveCompilationContext>true</PreserveCompilationContext>
    <CopyLocalLockFileAssemblies>true</CopyLocalLockFileAssemblies>
    <RuntimeIdentifiers> your target platform Runtime Identifiers </RuntimeIdentifiers>
  </PropertyGroup>
  ...
```

Note that:
1. You must build the sample on the platform you wish to target, as it depends on native assemblies built during Node package installation.
2. Running either the included publish-win or publish-linux package.json scripts will overwrite platform specific assemblies in `/dotNetAssemblies`.
3. Whe using the dotnet CLI to build and publish the sample .Net Core project, the `--self-contained` option must be included in the publish command if you wish to deploy to machines that do not have the dotnet framework installed. 


### Publishing for Windows runtime
From the command line in the project root run the following commands:

```
  npm install
  npm run publish-win
  npm start
```

Note: The publish-win script removes `ImageEditor.Core.deps.json` from the published files as it prevents correct composition of Software Potential assemblies on development machines.

#### To package the sample 
```
  npm run package-win
```
The output will be written to a directory called `Demo-win32-x64`.


### Publishing for Linux runtime
From the command line in the project root run the following commands:
```
  npm install
  npm run publish-linux
  npm start
```

#### To package the sample
```
  npm run package-linux
```
The output will be written to a directory called `Demo-linux-x64`.

## Using The Sample
### Activating a license

* Generate a license for the Product `Demo` and Version `1.0` via the [Software Potential service](https://srv.softwarepotential.com/Issue.aspx?IssueType=new) 
 * Create a license for product `Demo 1.0` (**Manage Licenses|Issue New Licenses** on the site)
  * Add a Feature (e.g., `Feature1`)

#### Online
* Select **Help > View Licenses** from the menu and click **Activate A License Using An Activation Key** to  bring up the activation form.menu).
* Select the Online tab and Paste the license key into the text box.
* Click Activate.

#### Offline
* Select **Help > View Licenses** from the menu and click **Activate A License Using An Activation Key** to  bring up the activation form.
* Select the Offline tab and Paste the license key into the text box.
* Click Generate to generate an activation request and save it to disk as a .txt file. This can then be processed via the [Software Potential Manual Activation page](http://manualactivation.softwarepotential.com)
* On a successful activation a License (.bin) file will be generated and made available for download.
* In the DemoApp open the Offline Tab of the activation form.
* Click Install and select the downloaded License file.

### Viewing the license list

* Select **Help > View Licenses** on the menu
* You can delete an activated license by clicking the **Delete** button beside each license

### Running protected code
Once a license is installed, options to execute features included in the license (**Convert To Greyscale**, **Rotate** or **Crop**) will appear in the menu under the Edit submenu.
Loading an image into the main window by selecting **File > Open** will enable available features.



## Troubleshooting

The most common problems reported with the sample are:

### Sample won't compile

* Ensure you have added the following NuGet packages to the `ImageEditor.Core` project:
 * `SoftwarePotential.Protection-<MyPermutationShortCode>`
 * `SoftwarePotential.Configuration.Local.SingleUser-<MyPermutationShortCode>` 
 * `SoftwarePotential.Licensing-<MyProduct_Version>`

### License will not activate

* Ensure _Locking_ is set to **Machine** (the .NET Standard `Sp.Agent` runtime does not support other binding styles)

### Packages not displaying in NuGet feed or installing correctly

* Ensure that your permutation version is 4.0.2003 or later on the Software Potential service [Develop > Manage Permutations page](https://srv.softwarepotential.com/Permutations.aspx).
    *  To update, select the permutation and click **Update**.
* Ensure that you have created the `Demo` product 

### License verification check fails with Sp.Agent.Internal.CoreServicesNotFoundException
* Ensure the `electron-edge-js` NPM module version is exactly 8.3.5 and that `ImageEditor.Core.deps.json` is not present in `\src\ImageEditor.Core\bin\publish\` if publishing for Windows.

## Documentation

Please see below for a list of recommended reading, ranging from Getting Started guides to more detailed technical documentation:

* [Getting Started - Licensing with Software Potential](https://support.softwarepotential.com/hc/en-us/articles/115001354529-Getting-Started-Licensing-with-Software-Potential)
* [Licensing-README](https://support.softwarepotential.com/hc/en-us/articles/115001358849-Licensing-README)
* [SoftwarePotential.Configuration.Local.SingleUser-README](https://support.softwarepotential.com/hc/en-us/articles/115001365849--SingleUser-Configuration-README)
* [Software Potential NuGet Feed](https://support.softwarepotential.com/hc/en-us/articles/115001371425-Getting-Started-Software-Potential-NuGet-Feed)
* [Software Potential APIs](http://api.softwarepotential.com/index.html)
* [electron-edge-js](https://github.com/agracio/electron-edge-js)
* [edge-js](https://github.com/agracio/edge-js)
* [Electron Documentation](https://electronjs.org/docs)
* [.NET Core Guide](https://docs.microsoft.com/en-us/dotnet/core/)

## Support
If you run into any issues visit the [Software Potential support page](https://support.softwarepotential.com) for more detailed information or to submit a help request.

Good luck!