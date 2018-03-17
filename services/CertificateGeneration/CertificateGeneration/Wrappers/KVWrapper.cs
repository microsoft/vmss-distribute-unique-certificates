using System;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Threading.Tasks;
using System.Web.Configuration;
using Microsoft.Azure.KeyVault;

namespace CertificateGeneration.Wrappers
{
    public class KVWrapper
    {
        public KVWrapper()
        {
            client = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(KVWrapper.GetToken));
        }

        //this is an optional property to hold the secret after it is retrieved
        //the method that will be provided to the KeyVaultClient
        private static async Task<string> GetToken(string authority, string resource, string scope)
        {
            var authContext = new AuthenticationContext(authority);
            ClientCredential clientCred = new ClientCredential(WebConfigurationManager.AppSettings["ClientId"],
                        WebConfigurationManager.AppSettings["ClientSecret"]);
            AuthenticationResult result = await authContext.AcquireTokenAsync(resource, clientCred);

            if (result == null)
                throw new InvalidOperationException("Failed to obtain the JWT token");

            return result.AccessToken;
        }

        public async Task UploadPem(string vaultBaseUrl, string secretName, string pem)
        {
            var bundle = await client.SetSecretAsync(vaultBaseUrl,
                                                 secretName,
                                                 pem);
        }

        public async Task UploadPfx(string vaultBaseUrl, string certificateName, string base64EncodedCertificate)
        {
            var bundle = await client.ImportCertificateAsync(vaultBaseUrl,
                                                     certificateName,
                                                     base64EncodedCertificate);
        }

        private KeyVaultClient client;
    }
}