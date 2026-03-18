namespace OWASP.Infrastructure.Repository;

using Microsoft.Azure.Cosmos;
using OWASP.Application.Interfaces;
using OWASP.Domain.Models;
using OWASP.Infrastructure.DataAccess;

public class CosmosRepository : IOvertimeEntryRepository
{
    private readonly CosmosDbContext _cosmosDb;

    public CosmosRepository(CosmosDbContext cosmosDb)
    {
        _cosmosDb = cosmosDb;
    }

    public async Task AddAsync(OvertimeEntry entry)
    {
        await _cosmosDb.Container.UpsertItemAsync(entry, new PartitionKey(entry.UserId.ToString()));
    }

    public async Task<OvertimeEntry?> GetByIdAsync(Guid userId, Guid entryId)
    {
        try
        {
            ItemResponse<OvertimeEntry> response = await _cosmosDb.Container.ReadItemAsync<OvertimeEntry>(
                entryId.ToString(), new PartitionKey(userId.ToString()));
            return response.Resource;
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task<IEnumerable<OvertimeEntry>> GetAllAsync(Guid userId)
    {
        var sql = "SELECT * FROM c WHERE c.UserId = @UserId";
        var queryDefinition = new QueryDefinition(sql)
            .WithParameter("@UserId", userId);

        var feedIterator = _cosmosDb.Container.GetItemQueryIterator<OvertimeEntry>(
            queryDefinition,
            requestOptions: new QueryRequestOptions { PartitionKey = new PartitionKey(userId.ToString()) });

        List<OvertimeEntry> results = new();

        while (feedIterator.HasMoreResults)
        {
            FeedResponse<OvertimeEntry> response = await feedIterator.ReadNextAsync();
            results.AddRange(response);
        }

        return results;
    }

    public async Task<IReadOnlyList<OvertimeEntry>> SearchAsync(Guid userId, DateOnly from, DateOnly to)
    {
        var sql = "SELECT * FROM c WHERE c.UserId = @UserId AND c.Date >= @From AND c.Date <= @To";
        var queryDefinition = new QueryDefinition(sql)
            .WithParameter("@UserId", userId)
            .WithParameter("@From", from.ToDateTime(TimeOnly.MinValue))
            .WithParameter("@To", to.ToDateTime(TimeOnly.MaxValue));

        var feedIterator = _cosmosDb.Container.GetItemQueryIterator<OvertimeEntry>(
            queryDefinition,
            requestOptions: new QueryRequestOptions { PartitionKey = new PartitionKey(userId.ToString()) });

        List<OvertimeEntry> results = new();

        while (feedIterator.HasMoreResults)
        {
            FeedResponse<OvertimeEntry> response = await feedIterator.ReadNextAsync();
            results.AddRange(response);
        }

        return results;
    }

    public async Task UpdateAsync(OvertimeEntry entry)
    {
        // Upsert acts as update in Cosmos DB
        await _cosmosDb.Container.UpsertItemAsync(entry, new PartitionKey(entry.UserId.ToString()));
    }

    public async Task DeleteAsync(Guid userId, Guid entryId)
    {
        try
        {
            await _cosmosDb.Container.DeleteItemAsync<OvertimeEntry>(
                entryId.ToString(), new PartitionKey(userId.ToString()));
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            // Item already deleted or does not exist, ignore
        }
    }

    public Task<IEnumerable<OvertimeEntry>> GetAllAsync(Guid id, Guid userId) => throw new NotImplementedException();
}
