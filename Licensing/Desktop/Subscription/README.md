# Subscription Desktop Sample application

This solution contains a simple project illustrating the following elements of a **Licensed** application in a **Desktop** environment:

* an Online Activation page
* expired subscription license(s) notification(s)
* manual renewal of expired subscription license(s)

The application uses WPF without any dependency on third party packages (e.g. MVVM frameworks).

# Configuring The Sample
## Create the product

* Navigate to the [Software Potential service product page](https://srv.softwarepotential.com/Products.aspx)
* Create a product entitled `Demo` with version `1.0` (case-sensitive) in a non-production account
  * Add two Features (`Feature1`,`Feature2`)

## Install the NuGet packages
* Add the SoftwarePotential NuGet feed (`https://srv.softwarepotential.com/NuGet/nuget/`) in your Package Manager settings
* Add the following package references:
 * `SoftwarePotential.Protection-<PermutationShortCode>` 
 * `SoftwarePotential.Configuration.Local.SingleUser-<PermutationShortCode>`
 * `SoftwarePotential.Licensing-Demo_10`

# Using The Sample
## Activating a license

* Generate a license for the Product `Demo` and Version `1.0` via the [Software Potential service](https://srv.softwarepotential.com/Issue.aspx?IssueType=new) 
 * Create a license for product `Demo 1.0` (**Manage Licenses|Issue New Licenses** on the site)
  * Add a Feature (e.g., `Feature1`)
* Paste the license key into the activation form (**Licenses|Activate** on the sample's menu)

## Renewing licenses

* Select **Licenses|Manage Licenses** on the menu
* Expired Subscription Licenses are displayed
* Click renew 

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

* [Getting Started with Subscription Licneses](http://docs.softwarepotential.com/Getting-Started-With-Subscription-Licenses.html)
* [Getting Started - Licensing with Software Potential](http://docs.softwarepotential.com/Getting-Started-With-Licensing.html)
* [Licensing-README](http://docs.softwarepotential.com/Licensing-README.html)
* [SoftwarePotential.Configuration.Local.SingleUser-README](http://docs.softwarepotential.com/Configuration.Local.SingleUser-README.html)
* [Software Potential NuGet Feed](http://docs.softwarepotential.com/Adding-SoftwarePotential-NuGet-Feed.html)

## Support
If you run into any issues, join us at `http://support.inishtech.com` on [the InishTech Support forum](http://www.inishtech.com/Support/Forum.aspx)

Good luck!
