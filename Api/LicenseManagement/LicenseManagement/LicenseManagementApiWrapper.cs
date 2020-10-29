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
using System.IdentityModel.Tokens;
using System.Net;
using System.ServiceModel;

namespace LicenseManagement
{
    // Allows correct execution of multiple API calls even if exceptions occur 
    // (which makes the LicenseManagementWSClient faulted, in which case a fresh one needs to be generated)
    // Enables one to have a single place in the code to load the credentials from e.g. a config file
    static class LicenseManagementApiFactory
    {
        public static LicenseManagementApi Create(string clientId, string clientSecret, string scope)
        {
            return new LicenseManagementApi(() => InternalCreateRaw(clientId, clientSecret, scope));
        }

        public static ILicenseManagementWS InternalCreateRaw(string clientId, string clientSecret, string scope)
        {
            string _url = SpApiConfiguration.BaseUrl + "SLMServerWS/LicenseManagementBearerWS.svc";
            //TO DO - REMOVE CERT VALIDATION CALL BACK WHEN NEW ENDPOINT RELEASED
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

            var token = JwtTokenHelper.GetWrappedAccessToken(clientId, clientSecret, scope);

            var binding = new WS2007FederationHttpBinding(WSFederationHttpSecurityMode.TransportWithMessageCredential);
            binding.HostNameComparisonMode = HostNameComparisonMode.Exact;
            binding.Security.Message.EstablishSecurityContext = false;
            binding.Security.Message.IssuerAddress = new EndpointAddress(SpApiConfiguration.Authority);
            binding.Security.Message.IssuedKeyType = SecurityKeyType.BearerKey;
            binding.MaxReceivedMessageSize = 2147483647;

            var factory = new ChannelFactory<ILicenseManagementWS>(binding, new EndpointAddress(_url));
            return factory.CreateChannelWithIssuedToken(token);
        }
    }
    class LicenseManagementApi
    {
        readonly Func<ILicenseManagementWS> _createClient;

        public LicenseManagementApi(Func<ILicenseManagementWS> createClient)
        {
            _createClient = createClient;
        }

        public TResult Execute<TResult>(Func<ILicenseManagementWS, TResult> serviceCalls)
        {
            var client = _createClient();
            try
            {
                return serviceCalls(client);
            }
            catch (Exception ex)
            {

                Console.WriteLine("LICENSE MANAGMENT API EXCEPTION: " + ex);
                ((IClientChannel)client).Abort();
                throw;
            }
            finally
            {
                ((IClientChannel)client).IfNotFaultedCloseAndCleanupChannel();

            }
        }
    }
    static class WcfExtensions
    {
        /// <summary>
        /// Safely closes a service client connection.
        /// </summary>
        /// <param name="client">The client connection to close.</param>
        public static void IfNotFaultedCloseAndCleanupChannel(this ICommunicationObject client)
        {
            // Don't try to Close if we are Faulted - this would cause another exception which would hide the primary one
            if (client.State == CommunicationState.Opened)
                try
                {
                    // Close this client
                    client.Close();
                }
                catch (CommunicationException)
                {
                    client.Abort();
                }
                catch (TimeoutException)
                {
                    client.Abort();
                }
                catch (Exception)
                {
                    client.Abort();
                    throw;
                }
        }
    }
}
