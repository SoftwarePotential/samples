/*
 * Copyright 2013-2021 (c) Inish Technology Ventures Limited.  All rights reserved.
 * 
 * This code is licensed under the BSD 3-Clause License included with this source
 * 
 * ALSO SEE: https://github.com/SoftwarePotential/samples/wiki/License
 * 
 */

// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.

using LicenseManagement.LicenseManagementWS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LicenseManagement
{
    class Program
    {
        static void Main(string[] args)
        {
            if ( args.Length !=2)
            {
                ShowUsage();
                return;
            }
            //SKU in testsoftwarepotential.com Test Account
            var testProduct = new CatalogProductEntry
            {
                Name = "Medical 2010 30 Day Eval",
                Description = "30 Day test trial/eval licenses",
                SkuTemplateLicenseKey = "2ABLY-YVNEA-DKKYF-SCWEM-7U2PK",
                SkuId = "DCF5C6F2-5B64-4DE9-9E23-42541D875F02"
            };
            LicenseManagementApi api = LicenseManagementApiFactory.Create(SpApiConfiguration.ClientId, SpApiConfiguration.ClientSecret, SpApiConfiguration.Scope);

            try
            {
                switch (args[0])
                {
                    case "-sku":
                        var skuId = args[1];
                        License newLicenseFromSku = CreateLicenseFromSkuId(api, skuId);
                        Console.WriteLine( $"License generated from SKU with activation key {newLicenseFromSku.ActivationKey} and license ID {newLicenseFromSku.LicenseId}");
                        break;
                    case "-get":
                        var activationKey = args[1];
                        License license = GetLicenseByActivationKey( api, activationKey );
                        Console.WriteLine($"License retrieved with activation key {license.ActivationKey}: license ID {license.LicenseId}, Issued at {license.LicenseInfo.IssueDate}");
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex);
            }
            Console.ReadLine();
        }
        public static License CreateLicenseFromSkuId(LicenseManagementApi api, string skuId)
        {

            return api.Execute(client =>
            {
                LicenseInfo licenseInfo = client.GetSkuById(skuId).LicenseInfo;
                License license = client.CreateLicense(licenseInfo);
                return license;
            });
        }
        public static License GetLicenseByActivationKey (LicenseManagementApi api, string activationKey)
        {

            return api.Execute(client =>
            {
                License license = client.GetLicenseByActivationKey(activationKey);
                return license;
            });
        }

        public static LicenseSummary GetLicenseSummaryById(LicenseManagementApi api, string licenseId)
        {

            return api.Execute(client =>
            {
                var licenseIds = new string[] { licenseId };
                LicenseSummary [] license = client.GetLicenseSummariesByLicenseIds(licenseIds);
                return license.FirstOrDefault();
            });
        }
        private static void ShowUsage()
        {
            throw new NotImplementedException();
        }

        public class CatalogProductEntry
        {
            public string Name { get; set; }
            public string Description { get; set; }

            //SkuId can be found on the SKU summary page
            public string SkuId { get; set; }

            //Sku Template License key can be found on the SKU summary page
            public string SkuTemplateLicenseKey { get; set; }
        }
    }
}
