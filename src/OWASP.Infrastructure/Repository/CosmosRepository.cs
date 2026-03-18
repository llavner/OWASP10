namespace OWASP.Infrastructure.Repository;

using Microsoft.Azure.Cosmos;

using OWASP.Application.Interfaces;
using OWASP.Infrastructure.DataAccess;

public class CosmosRepository(CosmosDbContext cosmosDb) : IOvertimeEntryRepository
{
    private readonly CosmosDbContext _cosmosDb = cosmosDb;

    public async Task UpsertRecordsAsync<T>(T record)
    {
        await _cosmosDb.Container.UpsertItemAsync(record);
    }

    public async Task<List<T>> LoadRecordsAsync<T>()
    {
        var sql = "select * from c";
        var queryDefinition = new QueryDefinition(sql);
        var feedIterator = _cosmosDb.Container.GetItemQueryIterator<T>(queryDefinition);

        List<T> records = new List<T>();

        while (feedIterator.HasMoreResults)
        {
            FeedResponse<T> currentResultSet = await feedIterator.ReadNextAsync();

            foreach (var record in currentResultSet)
            {
                records.Add(record);
            }
        }

        return records;
    }

    public Task<T> LoadRecordByIdAsync<T>(string id) => LoadRecordByPropertyAsync<T>("Id", id);

    public Task<T> LoadRecordByEmailAsync<T>(string email) => LoadRecordByPropertyAsync<T>("EmailAddress", email);

    public Task<T> LoadRecordByTokenAsync<T>(string token) => LoadRecordByPropertyAsync<T>("Token", token);

    public Task<T> LoadRecordByUserNameAsync<T>(string userName) => LoadRecordByPropertyAsync<T>("UserName", userName);

    private async Task<T> LoadRecordByPropertyAsync<T>(string propertyName, string value)
    {
        var sql = $"select * from c where c.{propertyName} = @Value";
        var queryDefinition = new QueryDefinition(sql).WithParameter("@Value", value);
        var feedIterator = _cosmosDb.Container.GetItemQueryIterator<T>(queryDefinition);

        while (feedIterator.HasMoreResults)
        {
            FeedResponse<T> currentResultSet = await feedIterator.ReadNextAsync();

            foreach (var record in currentResultSet)
            {
                return record;
            }
        }

        return default!;
    }
}
