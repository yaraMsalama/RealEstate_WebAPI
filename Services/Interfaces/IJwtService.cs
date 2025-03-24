using System.Security.Claims;

namespace RealEstate_WebAPI.Services.Interfaces
{
    public interface IJwtService
    {
       
            // Generates a JWT token for a given user
            string GenerateToken(string userId, string username, IEnumerable<string> roles);

            // Validates a JWT token and returns the principal if valid
            ClaimsPrincipal ValidateToken(string token);

            // Optionally, you can include a method to decode the token without validation
            IDictionary<string, object> DecodeToken(string token);
        
    }
}
