namespace OWASP.Infrastructure.Repository;

using Microsoft.Azure.Cosmos;

using OWASP.Domain.Interfaces;
using OWASP.Infrastructure.DataAccess;

public class CosmosRepository<T>
    where T : ICosmosEntity
{
    private readonly CosmosDbContext _cosmosDb;

    public CosmosRepository(CosmosDbContext cosmosDb)
    {
        _cosmosDb = cosmosDb;
    }

    public async Task AddAsync(T entry)
    {
        await _cosmosDb.Container.UpsertItemAsync(entry, new PartitionKey(entry.UserId.ToString()));
    }

    public async Task<T?> GetByIdAsync(Guid userId, Guid entryId)
    {
        try
        {
            ItemResponse<T> response = await _cosmosDb.Container.ReadItemAsync<T>(
                entryId.ToString(), new PartitionKey(userId.ToString()));
            return response.Resource;
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return default;
        }
    }

    public async Task<IEnumerable<T>> GetAllAsync(Guid userId)
    {
        var sql = "SELECT * FROM c WHERE c.UserId = @UserId";
        var queryDefinition = new QueryDefinition(sql)
            .WithParameter("@UserId", userId);

        var feedIterator = _cosmosDb.Container.GetItemQueryIterator<T>(
            queryDefinition,
            requestOptions: new QueryRequestOptions { PartitionKey = new PartitionKey(userId.ToString()) });

        List<T> results = new();

        while (feedIterator.HasMoreResults)
        {
            FeedResponse<T> response = await feedIterator.ReadNextAsync();
            results.AddRange(response);
        }

        return results;
    }

    public async Task<IReadOnlyList<T>> SearchAsync(Guid userId, DateOnly from, DateOnly to)
    {
        var sql = "SELECT * FROM c WHERE c.UserId = @UserId AND c.Date >= @From AND c.Date <= @To";
        var queryDefinition = new QueryDefinition(sql)
            .WithParameter("@UserId", userId)
            .WithParameter("@From", from.ToDateTime(TimeOnly.MinValue))
            .WithParameter("@To", to.ToDateTime(TimeOnly.MaxValue));

        var feedIterator = _cosmosDb.Container.GetItemQueryIterator<T>(
            queryDefinition,
            requestOptions: new QueryRequestOptions { PartitionKey = new PartitionKey(userId.ToString()) });

        List<T> results = new();

        while (feedIterator.HasMoreResults)
        {
            FeedResponse<T> response = await feedIterator.ReadNextAsync();
            results.AddRange(response);
        }

        return results;
    }

    public async Task UpdateAsync(T entry)
    {
        // Upsert acts as update in Cosmos DB
        await _cosmosDb.Container.UpsertItemAsync(entry, new PartitionKey(entry.UserId.ToString()));
    }

    public async Task DeleteAsync(Guid userId, Guid entryId)
    {
        try
        {
            await _cosmosDb.Container.DeleteItemAsync<T>(
                entryId.ToString(), new PartitionKey(userId.ToString()));
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            // Item already deleted or does not exist, ignore
        }
    }

    public Task<IEnumerable<T>> GetAllAsync(Guid id, Guid userId) => throw new NotImplementedException();
}
