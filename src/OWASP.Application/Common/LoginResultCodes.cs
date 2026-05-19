namespace OWASP.Application.Common;

public class LoginResultCodes
{
    public enum LoginResultCode
    {
        Success,
        UserNotFound,
        InvalidCredentials,
        UnknownError,
    }
}
