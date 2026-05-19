namespace OWASP.Application.Interfaces;

using OWASP.Application.Common;
using OWASP.Application.Dtos;
using OWASP.Domain.Models;

using static OWASP.Application.Common.OvertimeEntryResultCodes;

public interface IOvertimeEntryService
{
    Task<Result<List<OvertimeEntry>, OvertimeEntryResultCode>> GetAllEntriesAsync(string userId);

    Task<Result<OvertimeEntry, OvertimeEntryResultCode>> AddEntryAsync(string userId, OvertimeEntryRequest request);
}
