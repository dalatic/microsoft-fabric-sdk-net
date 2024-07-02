using Microsoft.Identity.Client;
using MicrosoftAPIPoC.Service.Models;

namespace MicrosoftAPIPoC.Service.Services
{
    public class AuthenticationService
    {
        private readonly AzureAdOptions _azureAdOptions;
        private readonly IPublicClientApplication _publicClientApp;

        public AuthenticationService(AzureAdOptions azureAdOptions)
        {
            _azureAdOptions = azureAdOptions;
            _publicClientApp = PublicClientApplicationBuilder.Create(_azureAdOptions.ClientId)
                .WithAuthority(_azureAdOptions.Authority)
                .WithRedirectUri(_azureAdOptions.RedirectUri)
                .Build();
        }

        public async Task<AuthenticationResult> AcquireTokenInteractiveAsync(string[] scopes)
        {
            return await _publicClientApp.AcquireTokenInteractive(scopes)
                .ExecuteAsync()
                .ConfigureAwait(false);
        }
    }
}
