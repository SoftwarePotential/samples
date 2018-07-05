# Electron Standalone Desktop Sample application

This solution contains a simple project illustrating the following elements of a **Licensed** Electron desktop application:

* an Online Activation page
* an Offline Activation page
* a diagnostic license list page
* licensed code execution

Protected features in a .NET assembly are called by Node.js code via [Edge.js](https://github.com/tjanczuk/edge).

## Prerequisites
* .NET Core 2.1.300 or later
* Node.js 8.2.1 or later
* Visual Studio Code / Visual Studio 2017 15.7 or later

# Configuring The Sample
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

# Publish and run app for Windows runtime
From the command line in the project root:

1. Install the node dependencies: **npm install**.
2. Publish the .Net assemblies: **npm run build-win**.
3. Start the app: **npm start**.

Note: The build-win script removes `ImageEditor.Core.deps.json` from the published files as it prevents correct composition of Software Potential assemblies on development machines.

## To Package the app for Windows runtime 
Use: **npm run package-win**. The output will be written to a directory called `Demo-win32-x64`.

# Using The Sample
## Activating a license

* Generate a license for the Product `Demo` and Version `1.0` via the [Software Potential service](https://srv.softwarepotential.com/Issue.aspx?IssueType=new) 
 * Create a license for product `Demo 1.0` (**Manage Licenses|Issue New Licenses** on the site)
  * Add a Feature (e.g., `Feature1`)

### Online
* Select **Help|View Licenses** from the menu and click **Activate A License Using A License Key** to  bring up the activation form.menu).
* Select the Online tab and Paste the license key into the text box.
* Click Activate.

### Offline
* Select **Help|View Licenses** from the menu and click **Activate A License Using A License Key** to  bring up the activation form.
* Select the Offline tab and Paste the license key into the text box.
* Click Generate to generate an activation request and save it to disk as a .txt file. This can then be processed via the [Software Potential Manual Activation page](http://manualactivation.softwarepotential.com)
* On a successful activation a License (.bin) file will be generated and made available for download.
* In the DemoApp open the Offline Tab of the activation form.
* Click Install and select the downloaded License file.

## Viewing the licenses list

* Select **Help|View Licenses** on the menu
* You can delete an activated license via the **Delete** button beside a license

## Running protected code
Once a license is installed, options to execute features included in the license (`Convert To Greyscale`, `Rotate` or `Crop`) will appear in the menu under the Edit submenu.
Loading an image into the main window by selecting **File|Open** will enable available features.


# Troubleshooting

The most common problems reported with the sample are:

## Sample won't compile

* Ensure you have added the following NuGet packages to the `ImageEditor.Core` project:
 * `SoftwarePotential.Protection-<PermutationShortCode>`
 * `SoftwarePotential.Configuration.Local.SingleUser-<PermutationShortCode>` 
 * `SoftwarePotential.Licensing-Demo_10`

## License will not activate

* Ensure _Locking_ is set to **Machine** (the .NET Standard `Sp.Agent` runtime does not support other binding styles)

## Packages not displaying in NuGet feed or installing correctly

* Ensure that your permutation version is 4.0.2003 or later on the [Software Potential service's permutations section in the account page](https://srv.softwarepotential.com/Permutations.aspx).
    *  To update, select the permutation and click **Update**.
* Ensure that you have created the `Demo` product 

## License verification check fails with Sp.Agent.Internal.CoreServicesNotFoundException
* Ensure the `electron-edge-js` NPM module version is exactly 8.3.5 and that `ImageEditor.Core.deps.json` is not present in `\src\ImageEditor.Core\bin\Release\netcoreapp2.1\win10-x64\publish\`.

## Documentation

Please see below for a list of recommended reading, ranging from Getting Started guides to more detailed technical documentation:

* [Getting Started - Licensing with Software Potential](https://support.softwarepotential.com/hc/en-us/articles/115001354529-Getting-Started-Licensing-with-Software-Potential)
* [Licensing-README](https://support.softwarepotential.com/hc/en-us/articles/115001358849-Licensing-README)
* [SoftwarePotential.Configuration.Local.SingleUser-README](https://support.softwarepotential.com/hc/en-us/articles/115001365849--SingleUser-Configuration-README)
* [Software Potential NuGet Feed](https://support.softwarepotential.com/hc/en-us/articles/115001371425-Getting-Started-Software-Potential-NuGet-Feed)
* [Software Potential APIs](http://api.softwarepotential.com/index.html)
* [Edge.js](https://github.com/tjanczuk/edge)
* [electron-edge-js](https://github.com/agracio/electron-edge-js)
* [.NET Core Guide](https://docs.microsoft.com/en-us/dotnet/core/)

## Support
If you run into any issues visit the [Software Potential support page](https://support.softwarepotential.com) for more detailed information or to submit a help request.

Good luck!
