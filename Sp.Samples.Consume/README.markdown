# Customer Sync Sample Application

## Prerequisites

* The sample requires .NET 4.0 on both your development machine and the end-user's machine
* The solution supports Visual Studio 2010 or later

### 0. Download the sample code onto your machine 

* you can [take it as a ZIP from here](https://github.com/SoftwarePotential/samples/zipball/master) 
* you can get it via git at https://github.com/SoftwarePotential/samples/

### 1. Build and configure the sample

* Launch powershell 
* Navigate to the Sp.Samples.Consume directory
* Run .\build
* Run .\configure -username:YourUserName -password:YourPassword

### 3. Using The Sample

* Run the application from a command prompt 
* Select one of the following two options
* CreateOrUpdate ExternalId CustomerName
* Delete ExternalId CustomerName
* e.g. Sp.Samples.Consume.CustomerSync.exe CreateOrUpdate ID1234 AcmeInc

## 4. More details available on the wiki

Further details are available on the wiki at https://github.com/SoftwarePotential/samples/wiki/Customer-Sync-Sample