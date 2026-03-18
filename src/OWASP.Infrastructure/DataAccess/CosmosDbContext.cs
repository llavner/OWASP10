namespace OWASP.Infrastructure.DataAccess;

using Microsoft.Azure.Cosmos;

public class CosmosDbContext
{
    private readonly string _endpointUrl;
    private readonly string _primaryKey;
    private readonly string _databaseName;
    private readonly string _containerName;
    private CosmosClient _cosmosClient;
    private Database _database;

    public Container Container { get; }

    public CosmosDbContext(string endpointUrl, string primaryKey, string databaseName, string containerName)
    {
        _endpointUrl = endpointUrl;
        _primaryKey = primaryKey;
        _databaseName = databaseName;
        _containerName = containerName;

        _cosmosClient = new CosmosClient(_endpointUrl, _primaryKey);
        _database = _cosmosClient.GetDatabase(_databaseName);

        Container = _database.GetContainer(_containerName);
    }
}
