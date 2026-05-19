namespace OWASP.Application.Interfaces;

using OWASP.Application.Common;
using OWASP.Application.Dtos;
using OWASP.Domain.Models;

using static OWASP.Application.Common.LoginResultCodes;

public interface IUserIdentityService
{
    Task<Result<User, LoginResultCode>> GetUserByName(string userName);

    Task<Result<User, LoginResultCode>> GetUserByEmail(string email);

    Task<Result<User, LoginResultCode>> Login(string username, string password);

    Task<Result<User, LoginResultCode>> Register(RegisterRequest regReq);
}
