namespace OWASP.Identity;

using Azure.Identity;

using OWASP.Infrastructure.DataAccess;

public static class Setup
{
    public static void AddIdentityDb(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IdentityDbContext>(provider =>
        {
            var config = builder.Configuration.GetSection("CosmosDb");
            var cosmosDb = new IdentityDbContext(
                config["EndpointUrl"],
                config["PrimaryKey"],
                config["DatabaseName"],
                config["IdentityContainerName"]);

            return cosmosDb;
        });
    }

    public static void AddKeyVaultSecrets(this WebApplicationBuilder builder)
    {
        var keyVaultUri = builder.Configuration["KeyVault:VaultUri"];
        builder.Configuration.AddAzureKeyVault(new Uri(keyVaultUri!), new DefaultAzureCredential());
    }
}
