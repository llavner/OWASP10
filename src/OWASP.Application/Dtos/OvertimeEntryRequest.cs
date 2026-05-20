namespace OWASP.Application.Dtos;

public class OvertimeEntryRequest
{
    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public int Hours { get; set; }

    public int Minutes { get; set; }
}
