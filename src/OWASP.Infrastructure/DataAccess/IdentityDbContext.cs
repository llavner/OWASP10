using Microsoft.Azure.Cosmos;

namespace OWASP.Infrastructure.DataAccess;

public class IdentityDbContext
{
    private readonly string _endpointUrl;
    private readonly string _primaryKey;
    private readonly string _databaseName;
    private readonly string _containerName;
    private readonly CosmosClient _cosmosClient;
    private readonly Database _database;

    public IdentityDbContext(string endpointUrl, string primaryKey, string databaseName, string containerName)
    {
        _endpointUrl = endpointUrl;
        _primaryKey = primaryKey;
        _databaseName = databaseName;
        _containerName = containerName;

        _cosmosClient = new CosmosClient(_endpointUrl, _primaryKey);
        _database = _cosmosClient.GetDatabase(_databaseName);

        Container = _database.GetContainer(_containerName);
    }

    public Container Container { get; }
}
