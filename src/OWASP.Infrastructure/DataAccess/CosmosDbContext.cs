namespace OWASP.Infrastructure.DataAccess;

using Microsoft.Azure.Cosmos;

public class CosmosDbContext
{
    private readonly string endpointUrl;
    private readonly string primaryKey;
    private readonly string databaseName;
    private readonly string containerName;
    private CosmosClient cosmosClient;
    private Database database;
    private Container container;

    public CosmosDbContext(string endpointUrl, string primaryKey, string databaseName, string containerName)
    {
        this.endpointUrl = endpointUrl;
        this.primaryKey = primaryKey;
        this.databaseName = databaseName;
        this.containerName = containerName;

        this.cosmosClient = new CosmosClient(this.endpointUrl, this.primaryKey);
        this.database = this.cosmosClient.GetDatabase(this.databaseName);
        this.container = this.database.GetContainer(this.containerName);
    }
}
