namespace OWASP.Overtime.Settings;

using Azure.Identity;

using OWASP.Infrastructure.DataAccess;

public static class Setup
{
    public static void AddOvertimeEntryDb(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<OvertimeEntryDbContext>(provider =>
        {
            var config = builder.Configuration.GetSection("CosmosDb");
            var cosmosDb = new OvertimeEntryDbContext(
                config["EndpointUrl"]!,
                config["PrimaryKey"]!,
                config["DatabaseName"]!,
                config["OvertimeEntryContainerName"]!);

            return cosmosDb;
        });
    }

    public static void AddKeyVaultSecrets(this WebApplicationBuilder builder)
    {
        var keyVaultUri = builder.Configuration["KeyVault:VaultUri"];
        builder.Configuration.AddAzureKeyVault(new Uri(keyVaultUri!), new DefaultAzureCredential());
    }
}
