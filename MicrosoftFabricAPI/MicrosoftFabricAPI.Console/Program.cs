using Microsoft.Extensions.Configuration;
using Microsoft.Fabric.Api;
using MicrosoftAPIPoC.Service.Models;
using MicrosoftAPIPoC.Service.Services;

namespace MicrosoftAPIPoC.ConsoleApp
{
    class Program
    {
        private static IConfiguration? Configuration;

        static async Task Main(string[] args)
        {
            // Configuración
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("Config/appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();

            // Verificar que la configuración se lee correctamente
            var azureAdOptions = Configuration.GetSection("AzureAd").Get<AzureAdOptions>();
            var fabricApiOptions = Configuration.GetSection("FabricApi").Get<FabricApiOptions>();

            // Servicio de autenticación
            var authService = new AuthenticationService(azureAdOptions);
            var result = await authService.AcquireTokenInteractiveAsync(fabricApiOptions.Scopes);

            Console.WriteLine("Access Token: " + result.AccessToken);

            // Uso del cliente de Fabric API
            var fabricClient = new FabricClient(result.AccessToken);

            // Obtener la lista de workspaces
            var workspaces = fabricClient.Core.Workspaces.ListWorkspaces().ToList();
            Console.WriteLine("Número de workspaces: " + workspaces.Count);
            foreach (var workspace in workspaces)
            {
                Console.WriteLine($"Workspace: {workspace.DisplayName}, Capacity ID: {workspace.CapacityId}");
            }

            // Obtener la lista de capacities
            var capacities = fabricClient.Core.Capacities.ListCapacities().ToList();
            Console.WriteLine("Número de capacities: " + capacities.Count);
            foreach (var capacity in capacities)
            {
                Console.WriteLine($"Capacity ID: {capacity.Id}, Display Name: {capacity.DisplayName}");
            }

        }
    }
}
