# License Issue Webhook Sample

This solution contains a project illustrating the following:

* Subscribing to a webhook notification
* Basic consumption of webhook data


# Configuring The Sample

## Install Nuget Packages
* `Microsoft.AspNet.WebHooks.Receivers.Custom`

## Register Webhook
* Navigate to the [Software Potential service webhook registration page](https://srv.softwarepotential.com/Develop/Webhook)
* Select Issue and Reissue actions
* Register the notification url `http://<yourdomain>/IssueNotifciation/api/webhooks/incoming/custom`
* *NB* the url must be publically accessible in order for Software Potential to contact it with a notification
* Enter a shared secret, must be between 32 and 64 characters. 
* *NB* Keep track of your shared secret using a password manager. This secret is your responsibility and cannot be retrieved from Software Potential.

## Configure credentials
* Enter the same shared secret as above in the web.config

## Running the sample
* Deploy the sample to a publically available url
* While the site is running, perform an issue and reissue of a license in Software Potential
* The `InMemoryLicenseNotificationRepository` will contain the basic license details
* The LicenseRefId and the optional CustomerId can then be used to retrieve more detailed information from Software Potential if required.

## Further reading
* https://docs.microsoft.com/en-ie/aspnet/webhooks/
