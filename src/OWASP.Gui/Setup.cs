namespace OWASP.Gui;

using Azure.Identity;

using OWASP.Infrastructure.DataAccess;

public static class Setup
{
    public static void AddCosmosDb(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<CosmosDbContext>(provider =>
        {
            var config = builder.Configuration.GetSection("CosmosDb");
            var endpointUrl = config["EndpointUrl"];
            var primaryKey = config["PrimaryKey"];
            var databaseName = config["DatabaseName"];
            var containerName = config["ContainerName"];

            if (string.IsNullOrWhiteSpace(endpointUrl))
            {
                throw new InvalidOperationException("CosmosDb:EndpointUrl configuration is missing.");
            }

            if (string.IsNullOrWhiteSpace(primaryKey))
            {
                throw new InvalidOperationException("CosmosDb:PrimaryKey configuration is missing.");
            }

            if (string.IsNullOrWhiteSpace(databaseName))
            {
                throw new InvalidOperationException("CosmosDb:DatabaseName configuration is missing.");
            }

            if (string.IsNullOrWhiteSpace(containerName))
            {
                throw new InvalidOperationException("CosmosDb:ContainerName configuration is missing.");
            }

            var cosmosDb = new CosmosDbContext(
                endpointUrl,
                primaryKey,
                databaseName,
                containerName);

            return cosmosDb;
        });
    }

    public static void AddKeyVaultSecrets(this WebApplicationBuilder builder)
    {
        var keyVaultUri = builder.Configuration["KeyVault:VaultUri"];
        if (string.IsNullOrWhiteSpace(keyVaultUri))
        {
            throw new InvalidOperationException("KeyVault:VaultUri configuration is missing.");
        }

        builder.Configuration.AddAzureKeyVault(new Uri(keyVaultUri), new DefaultAzureCredential());
    }
}
