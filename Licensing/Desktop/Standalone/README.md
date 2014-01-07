# Sp.Agent WPF Sample application

### What the sample is intended to demonstrate

This sample is intended to illustrate the required components of an application that:

* has a Desktop UI surface illustrating
 * an Online Activation form
 * a diagnostic license list form
* Click Once deployment
### What this sample is *not* intended to demonstrate 

* Windows Forms, ASP.NET, SaaS, Distributor integration

# Configure The Sample
## Create the product
* Navigate to the [Software Potential service](https://srv.softwarepotential.com/Products.aspx)
* Create a product entitled `Demo` with version `1.0` (case-sensitive) in a non-production account
  * Add two Features (`Feature1`,`Feature2`)

## Install the NuGet packages
* Add the SoftwarePotential NuGet feed: https://srv.softwarepotential.com/NuGet/nuget/ in your package manager console settings
* Add the following package references
 * SoftwarePotential.Protection-<PermutationShortCode> 
 * SoftwarePotential.Configuration.Local.SingleUser-<PermutationShortCode> 
 * SoftwarePotential.Licensing-Demo_10

# Using The Sample
## Activating a license

* Generate a license for the Product `Demo` and Version `1.0` via the [Software Potential service](https://srv.softwarepotential.com/Issue.aspx?IssueType=new) 
 * Create a license for product `Demo 1.0` (**Manage Licenses|Issue New Licenses** on the site)
  * Add a Feature (e.g., `Feature1`)
* Paste the license key into the activation form (**Licenses|Activate** on the sample's menu)

## Viewing the licenses list

* Select **Licenses|List Licenses** on the menu
* You can delete an activated license via the **Remove** button beside a license

## Running protected code

* Click the **Run Feature 1** button - this should display a success message
* Click the **Run Feature 2** button - this should display a denial message and navigate you to the activation dialog

# Troubleshooting

The most common problems reported with the sample are:

## Sample won't compile

* Ensure you have added the following NuGet packages
 * SoftwarePotential.Protection-`<PermutationShortCode>`
 * SoftwarePotential.Configuration.Local.SingleUser-`<PermutationShortCode>` 
 * SoftwarePotential.Licensing-Demo_10

## License will not activate

* Ensure _Locking_ is set to **Machine** (the `Sp.Agent` runtime does not support other binding styles)

## Packages Not Displaying in NuGet feed

* Your permutation may be too old to have packages associated with it.
 * Update your permutation on the [Software Potential service](https://srv.softwarepotential.com/Permutations.aspx)
* You have not created the demo product 

## Documentation

Please see below for a list of recommended reading, ranging from Getting Started guides to more detailed technical documentation

* [Getting Started - Licensing with Software Potential](http://docs.softwarepotential.com/Getting-Started-With-Licensing.html)
* [Licensing-README](http://docs.softwarepotential.com/Licensing-README.html)
* [SoftwarePotential.Configuration.Local.SingleUser-README](http://docs.softwarepotential.com/Configuration.Local.SingleUser-README.html)
* [Software Potential NuGet Feed](http://docs.softwarepotential.com/Adding-SoftwarePotential-NuGet-Feed.html)

## Support
If you run into any issues, join us at http://support.inishtech.com on [the InishTech Support forum](http://www.inishtech.com/Support/Forum.aspx)

Good luck!