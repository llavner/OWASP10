namespace OWASP.Application.Common;

public class OvertimeEntryResultCodes
{
    public enum OvertimeEntryResultCode
    {
        Success,
        NotFound,
        ValidationError,
        Unauthorized,
        Conflict,
        UnknownError,
    }
}
