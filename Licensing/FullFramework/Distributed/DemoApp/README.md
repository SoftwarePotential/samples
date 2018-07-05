## Using the Sample

### Basic Distributor Server setup
* Open an elevated command prompt
* Navigate to `Sp.Distributor-<PermutationShortCode>` within your NuGet packages folder.
* Run `.\Slps.Distributor.Host\install.cmd -install`

Alternatively you can create an MSI as outlined in the [Installer](https://github.com/SoftwarePotential/samples/tree/master/Licensing/FullFramework/Distributed/Installer) sample.
 
### Configure Licensing
* Run `DemoApp` project.
* Enter Distributor URL in the Licensing Configuration dialog, which will be displayed on the first application run
	-	for a locally installed Distributor Server, the default URL is `http://localhost:8731`
	-	click `Save`
* The Licensing Configuration dialog can always be accessed from **Licensing|Configure** on the sample's menu

### Issue and install a Distributed License
* Generate a Distributor license for the Product `Demo` and Version `1.0` via the [Software Potential service](https://srv.softwarepotential.com/Issue.aspx?IssueType=new) 
   * Add a Feature (e.g., `Feature1`)
   * Check **For use with Distributor only** checkbox
   * See [Getting Started With Distributor](https://support.softwarepotential.com/hc/en-us/articles/115001367189-Getting-Started-Distributor) for information how to install the license in the Distributor Server

### Activating a Local License
* Generate a license for the Product `Demo` and Version `1.0` via the [Software Potential service](https://srv.softwarepotential.com/Issue.aspx?IssueType=new) 
  * Add a Feature (e.g., `Feature2`)
* Paste the license key into the activation form (open **Licensing|Configure** on the sample's menu, then click **Activate**)

### Choosing a workflow type
The sample illustrates 2 different styles of consuming floating licenses with Software Potential Agent. The workflow/integration type can be selected on the Demo App startup screen.

* Declarative consumption - in this scenario feature allocation requests are made implicitly, at the point when the code that requires a license is encountered for the first time (see [Getting Started With Distributor - How it Works](https://support.softwarepotential.com/hc/en-us/articles/115001367189-Getting-Started-Distributor)). Licensed features are marked in the code with relevant attributes, the same way as in the [Standalone](https://github.com/SoftwarePotential/samples/tree/master/Licensing/FullFramework/Standalone) case; there's no need to use Software Potential Distributor API in the code.

* Acquire - in this scenario licensed features can be reserved up-front in a single request to the Distributor service. UI elements (buttons, menus) can be enabled or disabled based on whether respective features are held in current Software Potential Agent context (either have been obtained from the Distributor service, or from a local license)

### Running Protected Code

#### Declarative consumption scenario
* Click the **Run Feature 1** button - this should display a success message (we have a license for this feature in the Distributor Server)
* Click the **Run Feature 2** button - this should display a success message (we have a license for this feature installed locally)
* Click the **Run Feature 3** button - this should display a denial message

#### Acquire scenario
* Initially, only **Run Feature 2** button is enabled (as we have a license for this feature installed locally)
* Click the **Acquire** button. 
	* **Run Feature 1** button will get enabled (we have a license for this feature in the Distributor Server).
	* **Run Feature 3** button will stay disabled
* Clicking on **Run Feature 1** or **Run Feature 2** buttons should display a success message

### Checking out
* Launch the checkout dialog (**Licensing|Checkout** on the menu)
* Select an available checkout
* Enter the desired expiration date for the checkout
* Click **checkout**

### Checking in
* Launch the checkout dialog (**Licensing|Checkout** on the menu)
* Click **checkin**
