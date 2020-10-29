using IdentityModel;
using IdentityModel.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LicenseManagement
{
    class CachedTokenClient
    {
        private readonly object _lockObject = new object();

        private TokenResponse _cachedTokenResponse;
        private DateTime _timeToRefreshToken;
        private readonly string _authority;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _scope;
        private readonly HttpClient _httpClient;

        public CachedTokenClient(string authority, string clientId, string clientSecret, string scope)
        {
            _authority = string.IsNullOrEmpty(authority) ? throw new ArgumentNullException(nameof(authority)) : authority;
            _clientId = string.IsNullOrEmpty(clientId) ? throw new ArgumentNullException(nameof(clientId)) : clientId;
            _clientSecret = string.IsNullOrEmpty(clientSecret) ? throw new ArgumentNullException(nameof(clientSecret)) : clientSecret;
            _scope = string.IsNullOrEmpty(scope) ? throw new ArgumentNullException(nameof(scope)) : scope;
            _httpClient = new HttpClient();
        }

        public string GetAccessToken(bool forceFreshToken = false)
        {
            lock (_lockObject)
            {
                if (forceFreshToken || HasValidToken() == false)
                {
                    RefreshToken();
                }
                return _cachedTokenResponse.AccessToken;
            }
        }
        private void RefreshToken()
        {
            var client = new HttpClient();
            var disco = client.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = SpApiConfiguration.Authority.ToLower(),
                Policy = { RequireHttps = false }
            }).Result;

            if (disco.IsError)
                throw new Exception(disco.Error);

            var tokenResponse = client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = _clientId,
                ClientSecret = _clientSecret,
                Scope = _scope
            }).Result;
            if (tokenResponse.IsError)
                throw new Exception(tokenResponse.Error);
            _timeToRefreshToken = DateTime.Now.AddSeconds(tokenResponse.ExpiresIn);
            _cachedTokenResponse = tokenResponse;
            Console.WriteLine($"Token retrieved, expires in {tokenResponse.ExpiresIn} seconds");
        }

        private bool HasValidToken()
        {
            return _cachedTokenResponse != null && DateTime.Now.AddMinutes(-10) < _timeToRefreshToken;
        }

    }
}
