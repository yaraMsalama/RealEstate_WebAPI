using System.Security.Claims;

namespace RealEstate_WebAPI.Services.Interfaces
{
    public interface IJwtService
    {

        string GenerateToken(string userId, string username, IList<string> roles);

        //ClaimsPrincipal ValidateToken(string token);

        //IDictionary<string, object> DecodeToken(string token);

    }
}
