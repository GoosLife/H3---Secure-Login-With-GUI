using Microsoft.IdentityModel.Tokens;
using System.Drawing;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace H3___Secure_Login_With_GUI.Authentication
{
    public class JwtAuthenticator
    {
        

        public static string CreateUserToken(string guid, string username)
        {
            // Get secret key from environment variable
            string? jwtKey = Environment.GetEnvironmentVariable("jwtKey");

            if (jwtKey == null)
            {
                throw new Exception("JWT key not found in environment variables");
            }

            // Create a token
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(jwtKey); // Replace with your own secret key
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                        new Claim("guid", guid),
                        new Claim("username", username)
                    }),
                Expires = DateTime.UtcNow.AddDays(7), // Set the token expiration time
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }

        // Get user ID from token
        public static string GetClaim(string token, string claimKey)
        {
            // Get secret key from environment variable
            string? jwtKey = Environment.GetEnvironmentVariable("jwtKey");
            if (jwtKey == null)
            {
                throw new Exception("JWT key not found in environment variables");
            }
            // Create a token handler
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            // Set the validation parameters
            TokenValidationParameters validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtKey)),
                ValidateIssuer = false,
                ValidateAudience = false
            };
            // Validate the token
            ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
            // Get the user ID from the token
            Claim? claim = claimsPrincipal.FindFirst((string)claimKey);

            Console.WriteLine(claim!.Value);

            if (claim == null)
            {
                throw new Exception("Claim not found in token");
            }

            return claim.Value;
        }

        // Validate a token
        public static bool ValidateToken(string token)
        {
            // Get secret key from environment variable
            string? jwtKey = Environment.GetEnvironmentVariable("jwtKey");
            if (jwtKey == null)
            {
                throw new Exception("JWT key not found in environment variables");
            }
            // Create a token handler
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            // Set the validation parameters
            TokenValidationParameters validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtKey)),
                ValidateIssuer = false,
                ValidateAudience = false
            };
            // Validate the token
            try
            {
                ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
