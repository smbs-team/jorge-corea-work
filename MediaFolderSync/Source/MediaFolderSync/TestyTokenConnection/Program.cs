using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestyTokenConnection
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var cstr = string.Empty;
            try
            {
                string appId = "404b9b8b-5315-4ef3-859d-37352a12b2da";
                string appKey = "LgMtPZBof8RUhZO/Qa0T1r1gR2pKd/sjnOx3wni5Y4I=";
                string tenantId = "bae5059a-76f0-49d7-9996-72dfe95d69c7";

                /* The next four lines of code show you how to use AppAuthentication library to fetch secrets from your key vault */
                AzureServiceTokenProvider azureServiceTokenProvider = new AzureServiceTokenProvider($"RunAs=App;AppId={appId};TenantId={tenantId};AppKey={appKey}");
                KeyVaultClient keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));
                var secret = await keyVaultClient.GetSecretAsync("https://ptas-dev-keyvault.vault.azure.net/secrets/ptas-storage-connectionstring")
                        .ConfigureAwait(false);
                cstr = secret.Value;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                Console.Error.WriteLine(ex.InnerException?.Message);
                return;
            }


            try
            {
                var v = new PTASMediaHelperClasses.AzureStorageHelper(cstr);
                var x = await v.FileExists("media", "1/2/3/file.txt");
                Console.WriteLine(x);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                Console.Error.WriteLine(ex.InnerException?.Message);
            }
            Console.WriteLine("Press enter.");
            Console.ReadLine();
        }
    }
}
