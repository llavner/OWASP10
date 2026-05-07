namespace OWASP.Application.Interfaces;

public interface IUserIdentityRepository
{
    Task UpsertRecordsAsync<T>(T record);

    Task<List<T>> LoadRecordsAsync<T>();

    Task<T> LoadRecordByIdAsync<T>(string id);

    Task<T> LoadRecordByEmailAsync<T>(string email);

    Task<T> LoadRecordByTokenAsync<T>(string token);

    Task<T> LoadRecordByUserNameAsync<T>(string userName);
}
