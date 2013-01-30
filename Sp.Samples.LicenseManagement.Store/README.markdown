# Online Sales Application Overview
## What the sample is intended to demonstrate
This sample is intended to illustrate the required components of an application which:
* Facilitates an administrator to create, read, update/edit and delete entries to a catalogue of products 
	* The administrator must assign a valid Software Potential Sku Id to each catalogue entry. As such, the product details and associated Sku Id should relate to a similar product SKU created at the Software Potential portal.
* Facilitates a customer to view a list of products, select a product of interest and to ‘buy’ this product	
	* At the point of sale of a product, the Software Potential Web APIs are utilised to provide the customer with licensing information (Activation Key and License Id)

## What this sample is not intended to demonstrate
* Protection of features – this is demonstrated in the WpfApplicationWithInstaller application
* [SP.Agent](http://www.inishtech.com/),  [SaaS](http://www.inishtech.com/KB18), [Distributor](http://www.inishtech.com/KB22) integration

# Using the Sample
## Initial Set-up
* This sample currently uses credentials and products from a demonstration account on the Software Potential service.
* To get most out of this sample, however, it may be more appropriate to adjust the application to reflect your business, and some of your own products that you have created at the Software Potential service.
	* Include your Software Potential user credentials in the PurchaseService.cs class
	* Navigate through to the site administration section of the application and either create new catalogue entries to reflect your existing SKUs, or modify the catalogue entries currently stored as part of the application.

## Buying a product / Obtaining License Info
* Navigate to the ‘Buy’ section of the site
* Select and ‘buy’ a product from the list of products on offer
* License information is returned from the Software Potential Web Service (this sample application includes an Activation Key and a License Id) and added to a purchase record, which is displayed to the customer

# Troubleshooting
The most common problems reported with the sample are:
### License information is not obtained
* Ensure the user credentials you include in the 'PurchaseService.cs' class are accurate and for a valid Software Potential account
* Ensure that the account for which you included user credentials contains valid products and SKUs to make use of the Web APIs
