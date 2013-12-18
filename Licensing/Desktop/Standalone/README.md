# Sp.Agent WPF Sample application

### What the sample is intended to demonstrate

This sample is intended to illustrate the required components of an application which:

* maintains licenses in a shared repository on the machine in order to allow all users, regardless of the account they log in as on a given machine to invoke the application (i.e., it deals with the 'Install for all users' aspect of an installer vs just an 'Install for Me only' installation)
* has a Desktop UI surface illustrating
 * an Online Activation form
 * a diagnostic license list form
* Has a [Wix](http://wix.codeplex.com/) based installer that:
 * Correctly configures the licensing storage directory with appropriate access rights to enable any user on the machine to 
execute based on an installed license
 * Redistributes the relevant Sp.Agent DLLs
 * Has a .NET 4.0 installation check

### What this sample is *not* intended to demonstrate 

* Click Once deployment - lots of code falls away if you don't have the 'sharing licenses across user profiles' requirement
* Windows Forms - we intend to do a simpler sample which uses Windows Forms later
* ASP.NET, [SaaS](http://www.inishtech.com/KB18), [Distributor integration](http://www.inishtech.com/KB22)

# Using The Sample

## Activating a license

* Generate a license for the Product `WPF Sample` and Version `1.0` via the [Software Potential service](https://srv.softwarepotential.com)
 * Create a product entitled `WPF Sample` with version `1.0` (case-sensitive) in a non-production account
  * Add two Features (`FeatureA`,`FeatureB`,)
 * Create a license for product `WPF Sample 1.0` (**Manage Licenses|Issue New Licenses** on the site)
  * Add a Feature (e.g., `FeatureA`)
* Paste the license key into the activation form (**Licenses|Activate** on the sample's menu)

## Viewing the licenses list

* Select **Licenses|List Licenses** on the menu
* You can delete an activated license via the **Remove** button beside a license

## Running protected code

* Click the **Run Feature A** button - this should display a success message
* Click the **Run Feature B** button - this should display a denial message

# Troubleshooting

The most common problems reported with the sample are:

## Sample or Installer won't compile

* Ensure you have extracted the files to a directory near the root (so you don't go over the Windows 160 character folder name limit)
* Ensure you have WiX v 3.5 (e.g. v 3.6 won't work)
* Ensure you have the Software Potential SDK installed (Windows' **Programs and Features** should show _Software Potential Code Protector_)
* Ensure you have installed your `.SLMPermutation` file (see the Permutations tab in Code Protector)
* Ensure you have linked the sample to your permutation  (see the `ConfigureSample.cmd` link above)
* Ensure you have closed and reopened the solution (not individual projects); Visual Studio 2010 and later doesn't pick up changes to `.targets` files included from `.csproj` files etc. if you don't close the solution after running `ConfigureSample.cmd` 

## License will not activate

* Ensure _Locking_ is set to **Machine** (the `Sp.Agent` runtime does not support other binding styles)

## Installer reports .NET 4.0 is not installed

* The installer checks for the .NET 4.0 Full Profile; the Sp.Agent runtime requires .NET 4.0 or later (while `Sp.Agent` supports the .NET Client Profile, the installation check is for Full as we believe many WPF apps don't restrict themselves to the Client Profile)