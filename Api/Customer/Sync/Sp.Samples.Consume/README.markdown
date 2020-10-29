# Customer Sync Sample Application

## What the sample is intended to demonstrate

This sample is intended to illustrate the required components of an application which:

* create a customer using the Software Potential Rest Apis
* retrieve a customer using oData filtering to find a customer with a given externalId
* update an existing customer using the HAL links contained in the customer record retrieved from Software Potential
* delete an existing customer using the HAL links contained in the customer record retrieved from Software Potential

## Prerequisites

* The sample requires .NET 4.0 on both your development machine and the end-user's machine
* The solution supports Visual Studio 2017 or later
* You must register the sample application with Software Potential as per the guidance in [https://support.softwarepotential.com/hc/en-us/articles/360013903037](https://support.softwarepotential.com/hc/en-us/articles/360013903037). 
* For this sample you must register your application with the **consume** scope. 
* Copy the ClientID, ClientSecret and Scope properties as you will need these to configure the sample application.

### 0. Download the sample code onto your machine 

* You can [take it as a ZIP from here](https://github.com/SoftwarePotential/samples/zipball/master) 
* You can get it via git at https://github.com/SoftwarePotential/samples/

### 1. Build and configure the sample

* Launch Powershell 
* Navigate to the Sp.Samples.Consume directory
* Run .\build
* Run .\configure -clientid:YourClientId -clientsecret:YourClientSecret -scope: YourScope -Authority:YourAuthority -baseurl:YourBaseUrl

### 2. Using The Sample

* Run the application from a command prompt 
* Select one of the following three options
* Get ExternalId
* CreateOrUpdate ExternalId CustomerName
* Delete ExternalId CustomerName
* e.g. Sp.Samples.Consume.CustomerSync.exe CreateOrUpdate ID1234 AcmeInc
