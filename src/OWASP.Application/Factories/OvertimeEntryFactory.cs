namespace OWASP.Application.Factories;

using OWASP.Application.Dtos;
using OWASP.Application.Interfaces;
using OWASP.Domain.Models;

public class OvertimeEntryFactory : IOvertimeEntryFactory
{
    public OvertimeEntry Create(string userId, OvertimeEntryRequest request)
    {
        return new OvertimeEntry
        {
            id = Guid.NewGuid().ToString(),
            UserId = userId,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Hours = request.Hours,
            Minutes = request.Minutes,
        };
    }
}
