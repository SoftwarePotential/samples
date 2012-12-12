# Obfuscator Integration Sample Application

BuildIntegration - a sample application to demonstrate various aspects of build process integration using MSBuild

**See the wiki for more information: [BuildIntegration](https://github.com/SoftwarePotential/samples/wiki/BuildIntegration)**

## Prerequisites

An installed CodeProtector with an installed permutation. The samples must have been configured to use your permutation (see [Linking the sample to your permutation](https://github.com/SoftwarePotential/samples#1-linking-the-sample-to-your-permutation))

### 0. Download the sample code onto your machine 

* you can [take it as a ZIP from here](https://github.com/SoftwarePotential/samples/zipball/master) 
* you can get it via git or [github for Windows](http://windows.github.com) at https://github.com/SoftwarePotential/samples/

### 1. Linking the sample to your permutation

The repository just contains the source and relies on Code Protector and the Runtime DLLs from within your `.SLMPermutation` file. To link the sample to your specific Permutation, it is necessary to first embed the id into the build files by executing a batch file from the command prompt :- <code>ConfigureSample.cmd &lt;your permutation id number></code>

Example:
    <code>&lt;work area>\ConfigureSample.cmd 90c24107-d181-4542-a210-82112983711d</code>

**NB if you have the solution open in Visual Studio 2010, you MUST close and reopen the solution as MSBuild .targets files are cached (does not apply to VS 2012 or 2008).**

### 2. Executing the sample

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
