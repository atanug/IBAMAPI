using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IBAM.API.Helper{

public class AuthenticationInfo
{
    public bool IsValid { get; }
    public string UserId { get; }
    //public string Role { get; }

    public AuthenticationInfo(HttpRequest request)
    {
        //log.LogError(request.Headers);
        // Check if we have a header.
        if (!request.Headers.ContainsKey("Authorization"))
        {
            IsValid = false;

            return;
        }

        string authorizationHeader = request.Headers["Authorization"];

        // Check if the value is empty.
        if (string.IsNullOrEmpty(authorizationHeader))
        {
            IsValid = false;

            return;
        }

        // Check if we can decode the header.
        //IDictionary<string, object> claims = null;

        try
        {
            if (authorizationHeader.StartsWith("Bearer"))
            {
                authorizationHeader = authorizationHeader.Substring(7);
            }

            // // Validate the token and decode the claims.
            var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("Secret"));
                tokenHandler.ValidateToken(authorizationHeader, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                 UserId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value).ToString();
        }
        catch(Exception exception)
        {
            IsValid = false;

            return;
        }

        // Check if we have user claim.
        if (UserId=="")
        {
            IsValid = false;

            return;
        }

        IsValid = true;
        //UserId = Convert.ToString(claims["id"]);
        
    }
}
}