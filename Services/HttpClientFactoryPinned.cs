using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Income.Services
{
    public static class HttpClientFactoryPinned
    {
        private const string PinnedPublicKeyHash =
            "QHSy/kEJH7cLC4lmhDEAujDR3YqRJ7i0tpOLE20AYgw="; // full hash

        public static HttpClient Create(string baseAddress)
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback =
                    (req, cert, chain, errors) =>
                    {
                        if (errors != SslPolicyErrors.None || cert == null)
                            return false;

                        using var sha256 = SHA256.Create();
                        var hash = Convert.ToBase64String(
                            sha256.ComputeHash(cert.GetPublicKey())
                        );

                        return hash == PinnedPublicKeyHash;
                    }
            };

            var client = new HttpClient(handler)
            {
                BaseAddress = new Uri(baseAddress),
                Timeout = TimeSpan.FromSeconds(120)
            };

            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }
    }

}
