This is the master repository for Samples provided by the Software Potential support team.

There are indivual per-sample walk-throughs in individual Sample directories - this README only covers general topics around configuring and compiling the samples.

# Documentation 

Please refer to [the Samples wiki](https://github.com/SoftwarePotential/samples/wiki) for a walkthrough of the samples

# Before you start

## 1. Linking the samples to your permutation

The repository just contains the source and relies on Code Protector and the Runtime DLLs from within your `.SLMPermutation` file. To link the sample to your specific Permutation, it is necessary to first embed the id into the build files by executing a batch file from the command prompt :- <code>ConfigureSample.cmd &lt;your permutation id number></code>

Example:
    <code>&lt;work area> ConfigureSample.cmd 90c24107-d181-4542-a210-82112983711d</code>

**NB if you have the solution open in Visual Studio, you MUST close and reopen the _Solution_ as MSBuild .targets files are cached. This note applies to all versions of Visual Studio from 2008 up.**


## 2. Executing the WPF sample

Because the code is intended to be reliant on a shared license store, it is necessary for an appropriate folder to be created and shared under elevated permissions prior to the first run of the application. For this reason, you should build and execute the installer project prior to running the sample:-

* Build the `Sp.Samples.Agent.WpfApplicationInstaller` project
* Install the resulting `.MSI` file (one way to do that is to right click on the project and select **Open folder in Windows Explorer**, navigate to `<work area>\Sp.Samples.Agent.WpfApplicationWithInstaller\Sp.Samples.Agent.WpfApplicationInstaller\bin\Debug` and Open `Sp.Samples.Agent.WpfApplicationInstaller.msi`
* Select Software Potential WPF Sample from the start menu

## 3. The rest is on the wiki

Further details are at https://github.com/SoftwarePotential/samples/wiki 

## 4. Support

If you run into any issues, join us at http://support.inishtech.com on [the InishTech Support forum](http://www.inishtech.com/Support/Forum.aspx)

Good luck!