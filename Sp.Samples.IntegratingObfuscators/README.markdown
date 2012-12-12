# Obfuscator Integration Sample Application

## What the sample is intended to demonstrate how to 

* integrate a third-party obfuscation tool with a build process that uses CodeProtection. 
* use Strong-Naming with CodeProtection using snks and pfx files
* protect a project with multiple assemblies

## What this sample is *not* intended to demonstrate 

Many different Obfuscation tools are available on the market and we can't possibly show how to use each one of those. Having said that, we welcome contributions. 

## Prerequisites

An installed CodeProtector with an installed permutation. The samples must have been configured to use your permutation (see [Linking the sample to your permutation](https://github.com/SoftwarePotential/samples#1-linking-the-sample-to-your-permutation))

# Using The Sample

## 0. Download the sample code onto your machine 

* you can [take it as a ZIP from here](https://github.com/SoftwarePotential/samples/zipball/master) 
* you can get it via git or [github for Windows](http://windows.github.com) at https://github.com/SoftwarePotential/samples/

## 1. Linking the sample to your permutation

The repository just contains the source and relies on Code Protector and the Runtime DLLs from within your `.SLMPermutation` file. To link the sample to your specific Permutation, it is necessary to first embed the id into the build files by executing a batch file from the command prompt :- <code>ConfigureSample.cmd &lt;your permutation id number></code>

Example:
    <code>&lt;work area>\ConfigureSample.cmd 90c24107-d181-4542-a210-82112983711d</code>

**NB if you have the solution open in Visual Studio 2010, you MUST close and reopen the solution as MSBuild .targets files are cached (does not apply to VS 2012 or 2008).**

## 2. Executing the sample

Build and run the executable console project. You should see output similar to the following
```
MainObfuscatable is obfuscated: False
MainObfuscatable is protected: True
Assembly that declares MainObfuscatable has strong-name: True
GetAnswerObfuscatable is obfuscated: False
GetAnswerObfuscatable is protected: True
Assembly that declares GetAnswerObfuscatable has strong-name: True
The answer is: 42
```

# Sample Description

This sample has two projects: <code>Sp.Samples.IntegratingObfuscators</code> and <code>Sp.Samples.IntegratingObfuscators.Library</code>. Each assembly contains a method that can be protected and whose name can be obfuscated by an obfuscator. The executable assembly invokes a simple method on the library. During the execution, the program prints the following information about each method:
* Is the method obfuscated?
* Is the method protected?
* Is the assembly strong-named?

The projects in this solution have been configured for protection and strong-naming using the instructions below.

## Integrating protection
When CodeProtector is installed, we install an MSBuild extension called `Slps.Protector.targets` that hooks into the build process. To enable code protection on a project:
* add a `<YourProjectName>.SLMCfg` to the root of your project (you can add it to your project if you wish, but its not necessary and you don't need to configure it for copying to the output directory)
* open the Project Properties, select the **Build** tab and choose **All Configurations** in the Configuration drop-down box at the top. In **Conditional compilation symbols** add `SLPS_PROTECT`

## Integrating obfuscation
Obfuscation must happen _before_ CodeProtection. We provide an MSBuild target named `BeforeSpProtect` that you can override to apply transformations on the compiled assembly before protection. In this case, we demonstrate how to an obfuscator. To override this target, simply declare a target with the same name _after_ `Microsoft.CSharp.targets` is imported into your project. If you want to use obfuscation on multiple projects, we recommend using a .target file that defines your custom `Slps_Obfuscate` **(TODO update)** target and import that from each project (again, after `Microsoft.CSharp.targets`). In this sample we use this approach, the target is called `Sp.Obfuscation.Custom.targets`. **(TODO we should use a convention based targets filename)**

An `BeforeSpProtect` target can use the following Items and Properties:
* the input assembly is at `@(SpProtectInputAssembly)`, in the directory `$(SpProtectBeforePath)`
* the input assemblys' PDB is at `@(SpProtectInputAssemblyPdb)`, in the directory `$(SpProtectBeforePath)`

## Integrating strong-naming into a project

CodeProtection transparently works with projects configured for strong naming using the Visual Studio Project Properties. No further steps should be necessary for a customer. However we do believe the following key points are very important to bear in mind if you intend to strong-name protected assemblies.

### ...using .snk files
In this sample `Sp.Samples.IntegratingObfuscators.Library` is strong-named using a .snk file. To configure a project for strong naming using an snk file, do the following:

* In the `.SLMCfg` file, ensure `SkipResign` is set to `false` (this is a legacy setting and that should be the default)
* In the Project Properties, select the **Signing** Tab and enable **Sign the assembly** with the given key pair file

### ...using .pfx files
When you choose to password protect your strong name key file using a password, Visual Studio will generate a `.pfx` file instead of a `.snk` file. The first time you build a project that should be strong-named with a `.pfx` key, Visual Studio will prompt you for this key and store it in the machine's Crypto Service Provider Key-Container in a 'well-known' location that involves a hash of the keyfile, the domain and username. The container name generated in this manner looks like `VS_KEY_XXXX`. CodeProtection will use the private key stored in this location to sign protected assemblies.

If you examine `Sp.Samples.IntegratingObfuscators.csproj`, you may notice one oddity. We manually import the pfx key using a PowerShell script `InstallPfx.ps1` into a different CSP Container and then override an internal MSBuild target. This is done so we can build and run tests against these sample projects in our own Continuous Integration environment.

For your own projects, **we do NOT recommend** using something like our `InstallPfx.ps1` to install pfx keys in an automated fashion as it perverts the idea of having a password-protected private key in the first place. Instead, an administrator with the permission to access the private key should manually import the pfx on the build machines into a known CSP Container using the [sn-tool](http://msdn.microsoft.com/en-us/library/k5b5tt23(v=vs.110).aspx). You can then use the technique we use to override the `ResolveKeySource` target to feed in this key container name. 

Note that this is a very advanced technique and Microsoft does not provide an official way to use pfx files in a CI setup. Typically, people manually import the pfx on their build machines into the `VS_KEY_XXXX` folder (which is an alternative, but remember the XXXX hash may change and you will have to redo this step then). 

## Troubleshooting

<code>Cryptographic failure while signing assembly ...Sp.Samples.IntegratingObfuscators.exe' -- 'The key container name 'SpSampleCspContainer' does not exist'</code>

Visual Studio must be run with elevated (Administrator) privileges to allow storing the private signing key from the pfx in the machines CSP store. This is a general requirement when working with pfx keys in MSBuild.