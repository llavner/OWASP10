namespace OWASP.Application.Interfaces;

using OWASP.Application.Dtos;
using OWASP.Domain.Models;

public interface IOvertimeEntryFactory
{
    OvertimeEntry Create(string userId, OvertimeEntryRequest request);
}
