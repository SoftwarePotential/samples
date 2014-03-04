
##Using the Sample

### Basic Distributor Server setup
* Open an elevated command prompt
* Navigate to `Sp.Distributor-<PermutationShortCode>` within your NuGet packages folder.
* Run .\Slps.Distributor.Host\install.cmd -install

Alternatively you can create an MSI as outlined in the [Installer](https://github.com/SoftwarePotential/samples/tree/master/Licensing/Desktop/Distributed/Installer) sample.
 
### Configure Licensing
* Run `DemoApp` project.
* Enter Distributor URL in the Licensing Configuration dialog, which will be displayed on the first application run
	-	for a locally installed Distributor Server, the default URL is `http://localhost:8731`
	-	click `Save`
* The Licensing Configuration dialog can always be accessed from **Licensing|Configure** on the sample's menu

### Issue and install a network license
* Generate a Distributor license for the Product `Demo` and Version `1.0` via the [Software Potential service](https://srv.softwarepotential.com/Issue.aspx?IssueType=new) 
   * Add a Feature (e.g., `Feature1`)
   * Check **For use with Distributor only** checkbox
   * See _Getting Started With Distributor_ for information how to install the license in the Distributor Server

### Activating a local license
* Generate a license for the Product `Demo` and Version `1.0` via the [Software Potential service](https://srv.softwarepotential.com/Issue.aspx?IssueType=new) 
  * Add a Feature (e.g., `Feature2`)
* Paste the license key into the activation form (open **Licensing|Configure** on the sample's menu, then click **Activate**)

### Running protected code
* Click the **Run Feature 1** button - this should display a success message (we have a license for this feature in the Distributor Server)
* Click the **Run Feature 2** button - this should display a success message (we have a license for this feature installed locally)
* Click the **Run Feature 3** button - this should display a denial message and navigate you to the activation dialog
