namespace OWASP.Infrastructure.Repository;

using Microsoft.Azure.Cosmos;

using OWASP.Application.Interfaces;
using OWASP.Domain.Interfaces;
using OWASP.Infrastructure.DataAccess;

public class OvertimeEntryRepository(OvertimeEntryDbContext cosmosDb) : IOvertimeEntryRepository
{
    private readonly OvertimeEntryDbContext _cosmosDb = cosmosDb;

    public async Task UpsertRecordsAsync<T>(T record)
    where T : ICosmosEntity
    {
        if (string.IsNullOrEmpty(record.UserId.ToString()))
        {
            throw new ArgumentException("UserId must not be null or empty for partition key.");
        }

        await _cosmosDb.Container.UpsertItemAsync(record);
    }

    public async Task<List<T>> LoadRecordsByUserIdAsync<T>(string userId)
    {
        return await LoadRecordsByPropertyAsync<T>("UserId", userId);
    }

    private async Task<List<T>> LoadRecordsByPropertyAsync<T>(string propertyName, string value)
    {
        var sql = $"select * from c where c.{propertyName} = @Value";
        var queryDefinition = new QueryDefinition(sql).WithParameter("@Value", value);
        var feedIterator = _cosmosDb.Container.GetItemQueryIterator<T>(queryDefinition);

        List<T> results = [];

        while (feedIterator.HasMoreResults)
        {
            var currentResultSet = await feedIterator.ReadNextAsync();
            results.AddRange(currentResultSet);
        }

        return results;
    }
}
