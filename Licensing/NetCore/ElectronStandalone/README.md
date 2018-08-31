# Electron Standalone Desktop Sample application

This sample provides an illustration of the following elements of a **Licensed** Electron desktop application:

* an Online Activation page
* an Offline Activation page
* a license list page
* licensed code execution whereby protected features in a .NET assembly are run in an Electron app via the [electron-edge-js](https://github.com/agracio/electron-edge-js) fork of Edge.js.

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

### Create a Software Potential Product
  * Navigate to the Software Potential service [Define > Create Products page](https://srv.softwarepotential.com/Products.aspx)
  * Create a Product entitled `Demo` with version `1.0` (case-sensitive) in a non-production account
  * Add three Features (`Feature1`, `Feature2` and `Feature3`)

These Product and Feature definitions correspond to selected functionality in the sample source code, and licensing is implemented in the code with the use of declarative Software Potential Protection Attributes. Functions decorated with these attributes will only execute if a valid license which includes a particular defined Feature has been activated (see [Getting Started - Licensing with Software Potential](https://support.softwarepotential.com/hc/en-us/articles/115001354529-Getting-Started-Licensing-with-Software-Potential) for a more detailed guide to Declarative Licensing).

In the sample, the functions which execute functionality corresponding to Features defined in the service (i.e. `Feature1`, `Feature2` and `Feature3`) are called `MutateGreyscale`, `MutateRotate` and `MutateCrop`, and are located in `src/ImageEditor.Core/Features.cs`.

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

## Build, publish and package the sample
The sample can be published, packaged and run on the command line using npm scripts included in the `package.json` file.

### Specifiying the target platform
The included publish scripts target either Windows 10 64 bit or Ubuntu 18.04 64 bit platforms out of the box.
If you wish to target different versions of Windows or Linux you should:
1. Ascertain the target platform's .Net Core [Runtime Identifier](https://docs.microsoft.com/en-us/dotnet/core/rid-catalog) (RID) by running dotnet --info on the command line.
2. Set the -r argument value to the target platform Runtime Identifier for appropriate publish commands run by the `publish-win` and/or `publish-linux` scripts in  `package.json`:
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
2. Running either the included `publish-win` or `publish-linux` npm scripts will overwrite platform specific assemblies in `/dotNetAssemblies`.
3. If using the dotnet CLI to build and publish the sample .Net Core project, the `--self-contained` option must be included in the publish command if you wish to deploy to machines that do not have the dotnet framework installed. 


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
* Navigate to the Software Potential service  [Issue > Custom page](https://srv.softwarepotential.com/Issue.aspx?IssueType=new),
* Create a license for product `Demo 1.0`.
* Add a Feature (e.g., `Feature1`)
* If you wish to associate a named software edition (e.g. Image Editor Professional Edition) with your feature set to be displayed in the product description on the **About** page, add a custom tag with the key set to "Edition" and the value set to the edition name (e.g. "Professional"). 

#### Online
* Select **Help > View Licenses** from the menu and click **Activate A License Using An Activation Key** to  bring up the activation form.
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

## Using the sample with a pre-existing Product
Out of the box, the sample can be used with a Product called `Demo` version `1.0` with Features `Feature1`, `Feature2` and `Feature3`. However, if you opt to use an existing product you will need to
1. Edit the Software Potential Protection Attributes in the .Net source code:
  * Open `src/ImageEditor.Core/Features.cs`.
  * Replace attributes referencing `Demo_10`, `Feature1`, `Feature2` and `Feature3` with your selected product and features e.g.
  ```
  [MyExistingProduct_Version.Features.MyFeature]
  void MutateGreyscale( Image<Rgba32> image ) => image.Mutate( x => x.Grayscale() );
  ```
2. Modify the menu item mapping so that licensed features will be included in the Edit submenu:
  * Open `menu.js`
  * Replace `Feature1`, `Feature2` and `Feature3` with your product's features, e.g: 
  ```
    _addLicensedMenuItems.set(this, (menu, features) => {
              const createItem = _createMenuItemFromLabel.get(this);

              const editSubmenu = menu.items[1].submenu;
              editSubmenu.append(new MenuItem({
                  type: 'separator'
              }));

              if (features.includes('<MyFeatureX>')) editSubmenu.append(createItem('Convert To Greyscale'));
              if (features.includes('<MyFeatureY>')) {
                  editSubmenu.append(createItem('Rotate CW'));
                  editSubmenu.append(createItem('Rotate CCW'));
              }
              if (features.includes('<MyFeatureZ>')) editSubmenu.append(createItem('Crop'));
          });
  ```
3. Issue a license for the desired Product and Features in the Software Potential Service.
4. Publish the sample and activate the license as described in **Activating a license** above.

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
* [electron-packager](https://github.com/electron-userland/electron-packager)
* [Electron Documentation](https://electronjs.org/docs)
* [.NET Core Guide](https://docs.microsoft.com/en-us/dotnet/core/)

## Support
If you run into any issues visit the [Software Potential support page](https://support.softwarepotential.com) for more detailed information or to submit a help request.

Good luck!
