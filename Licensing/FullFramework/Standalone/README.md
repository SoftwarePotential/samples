# Standalone Desktop Sample application

This solution contains a simple project illustrating the following elements of a **Licensed** application in a **Desktop** environment:

* an Online Activation page
* an Offline Activation page
* a diagnostic license list page
* Click Once deployable

## Prerequisites
* .NET 4.0 or later
* Visual Studio 2010 or later
* [NuGet Package Manager](http://visualstudiogallery.msdn.microsoft.com/27077b70-9dad-4c64-adcf-c7cf6bc9970c) 2.5 or later

# Configuring The Sample
## Create the product

* Navigate to the [Software Potential service product page](https://srv.softwarepotential.com/Products.aspx)
* Create a product entitled `Demo` with version `1.0` (case-sensitive) in a non-production account
  * Add two Features (`Feature1`,`Feature2`)

## Install the Software Potential NuGet packages
* Add the SoftwarePotential NuGet feed (`https://srv.softwarepotential.com/NuGet/nuget/`) in your Package Manager settings
* Add the following package references:
 * `SoftwarePotential.Protection-<PermutationShortCode>` 
 * `SoftwarePotential.Configuration.Local.SingleUser-<PermutationShortCode>`
 * `SoftwarePotential.Licensing-Demo_10`

## Dependencies
The application uses WPF without any dependency on third party packages (e.g. MVVM frameworks).
However the QR code generation for Offline Activation requires the installation of the QRCoder [NuGet package](https://www.nuget.org/packages/QRCoder/).

# Using The Sample
## Activating a license

* Generate a license for the Product `Demo` and Version `1.0` via the [Software Potential service](https://srv.softwarepotential.com/Issue.aspx?IssueType=new) 
 * Create a license for product `Demo 1.0` (**Manage Licenses|Issue New Licenses** on the site)
  * Add a Feature (e.g., `Feature1`)

### Online
* Paste the license key into the Online Tab of the activation form (**Licenses|Activate** on the sample's menu)
* Click Activate

### Offline
* Paste the license key into the Offline Tab of the activation form (**Licenses|Activate** on the sample's menu)
* Click Generate
* EITHER scan the generated QR code to process an activation request automatically 
* OR click Save to save an activation request as a .txt file to disk. This can then be processed via the [Software Potential Manual Activation page](http://manualactivation.softwarepotential.com)
* On a successful activation a License (.bin) file will be generated and made available for download.
* In the DemoApp open the Offline Tab of the activation form (Licenses|Activate on the sample's menu)
* Click Install and select the downloaded License file

## Viewing the licenses list

* Select **Licenses|List Licenses** on the menu
* You can delete an activated license via the **Remove** button beside a license

## Running protected code

* Click the **Run Feature 1** button - this should display a success message
* Click the **Run Feature 2** button - this should display a denial message and navigate you to the activation dialog

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
