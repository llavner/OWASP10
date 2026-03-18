namespace OWASP.Infrastructure.Repository;

using OWASP.Application.Interfaces;

public class CosmosRepository : IOvertimeEntryRepository
{
    public Task<T> LoadRecordByEmailAsync<T>(string email) => throw new NotImplementedException();

    public Task<T> LoadRecordByIdAsync<T>(string id) => throw new NotImplementedException();

    public Task<T> LoadRecordByTokenAsync<T>(string token) => throw new NotImplementedException();

    public Task<T> LoadRecordByUserNameAsync<T>(string userName) => throw new NotImplementedException();

    public Task<List<T>> LoadRecordsAsync<T>() => throw new NotImplementedException();

    public Task UpsertRecordsAsync<T>(T record) => throw new NotImplementedException();
}
