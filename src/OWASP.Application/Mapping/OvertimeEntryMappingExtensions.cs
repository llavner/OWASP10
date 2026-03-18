using OWASP.Application.Dtos;
using OWASP.Domain.Models;

namespace OWASP.Application.Mapping;

public static class OvertimeEntryMappingExtensions
{
    public static OvertimeEntry Map(this OvertimeEntryCreate dto)
    {
        return new OvertimeEntry(
            Guid.NewGuid(),
            dto.UserId,
            dto.Date.ToDateTime(TimeOnly.MinValue),
            dto.Minutes,
            dto.Description,
            dto.Category,
            DateTimeOffset.UtcNow,
            DateTimeOffset.Now);
    }

    public static OvertimeEntry Map(this OvertimeEntryResponse dto)
    {
        return new OvertimeEntry(
            dto.Id,
            dto.UserId,
            dto.Date.ToDateTime(TimeOnly.MinValue),
            dto.Minutes,
            dto.Description,
            dto.Category,
            DateTimeOffset.UtcNow,
            DateTimeOffset.Now);
    }

    public static OvertimeEntry Map(this OvertimeEntryUpdate dto)
    {
        return new OvertimeEntry(
            dto.Id,
            dto.UserId,
            dto.Date.ToDateTime(TimeOnly.MinValue),
            dto.Minutes,
            dto.Description,
            dto.Category,
            DateTimeOffset.UtcNow,
            DateTimeOffset.Now);
    }
}
