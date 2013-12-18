# Sp.Agent WPF Sample application

WpfSample - WPF-based desktop application with a .MSI installer

## Prerequisites

* The sample requires .NET 4.0 on both your development machine and the end-user's machine
* The solution supports Visual Studio 2010 or later
* [Wix v3.5](http://wix.codeplex.com/releases/view/60102) should be installed on the machine in order to be able to build or load the Installer project (either on the commandline or in Visual Studio)
* You'll need to have the Software Potential SDK and your Permutation installed (see [the Getting Started Guide](http://support.inishtech.com/Support/Documentation/Technical-Articles/30-Day-Trial-Quick-Start-Guide.aspx))
### 0. Download the sample code onto your machine 

* you can [take it as a ZIP from here](https://github.com/SoftwarePotential/samples/zipball/master) 
* you can get it via git at https://github.com/SoftwarePotential/samples/

### 1. Linking the sample to your permutation

The repository just contains the source and relies on Code Protector and the Runtime DLLs from within your `.SLMPermutation` file. To link the sample to your specific Permutation, it is necessary to first embed the id into the build files by executing a batch file from the command prompt :- <code>ConfigureSample.cmd &lt;your permutation id number></code>

Example:
    <code>&lt;work area>\ConfigureSample.cmd 90c24107-d181-4542-a210-82112983711d</code>

**NB if you have the solution open in Visual Studio 2010 or earlier, you MUST close and reopen the solution as MSBuild .targets files are cached.**


### 2. Executing the sample

Because the code is intended to be reliant on a shared license store, it is necessary for an appropriate folder to be created and shared under elevated permissions prior to the first run of the application. For this reason, you should build and execute the installer project prior to running the sample:-

* Build the `Sp.Samples.Agent.WpfApplicationInstaller` project
* Install the resulting `.MSI` file (one way to do that is to right click on the project and select **Open folder in Windows Explorer**, navigate to `<work area>\Sp.Samples.Agent.WpfApplicationWithInstaller\Sp.Samples.Agent.WpfApplicationInstaller\bin\Debug` and Open `Sp.Samples.Agent.WpfApplicationInstaller.msi`
* Select Software Potential WPF Sample from the start menu
