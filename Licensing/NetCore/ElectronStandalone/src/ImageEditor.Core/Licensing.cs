/*
 * Copyright (c) Inish Technology Ventures Limited.  All rights reserved.
 * 
 * This code is licensed under the BSD 3-Clause License included with this source
 * 
 * ALSO SEE: https://github.com/SoftwarePotential/samples/blob/master/License.txt
 * 
 */
using Sp.Agent;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ImageEditor.Core
{
    public class Licensing
    {
        public async Task<object> VerifyStoresInitialized(dynamic input)
        {
            SpAgent.Configuration.VerifyStoresInitialized();
            return true;
        }

        public async Task<object> Activate(dynamic input)
        {
            await SpAgent.Product.Activation.OnlineActivateAsync((string)input);
            return true;
        }

        public async Task<object> IsActivationKeyWellFormed(dynamic input) =>
            SpAgent.Product.Activation.IsWellFormedKey((string)input);

        public async Task<object> GenerateActivationRequest(dynamic input) =>
            SpAgent.Product.Activation.Advanced().CreateManualActivationRequest((string)input, null);

        public async Task<object> InstallLicenseFile(dynamic input)
        {
            var license = File.ReadAllBytes((string)input);
            SpAgent.Product.Stores.Install(license);
            return true;
        }

        public async Task<object> GetProductName(dynamic input) => SpAgent.Product.ProductName;

        public async Task<object> GetProductVersion(dynamic input) => SpAgent.Product.ProductVersion;

        public async Task<object> RetrieveAllLicenses(dynamic input) =>
            from license in SpAgent.Product.Licenses.All()
            select new License(license.ActivationKey, license.ValidUntil, license.Advanced.AllFeatures().Select(f => f.Key).ToArray());

        public async Task<object> DeleteLicenseByActivationKey(dynamic input)
        {
            SpAgent.Product.Stores.Delete((string)input);
            return true;
        }

        public async Task<object> GetFeatures(dynamic input) =>
            SpAgent.Product.LocalFeatures.Valid().Except(new HashSet<string> { "Execute" }).ToArray();

        public async Task<object> GetEdition(dynamic input)
        {
            var validLicenses = SpAgent.Product.Licenses.Valid();
            if (!validLicenses.Any()) return "Unlicensed";

            return validLicenses
               .Where(x => x.Tags.ContainsKey("Edition"))
               .Select(x => x.Tags["Edition"])
               .FirstOrDefault();
        }

        class License
        {
            public string ActivationKey { get; set; }
            public DateTime ValidUntil { get; set; }
            public IEnumerable<string> Features { get; set; }

            public License(string activationKey, DateTime validUntil, string[] features)
            {
                ActivationKey = activationKey;
                ValidUntil = validUntil;
                Features = features;
            }
        }
    }
}
