using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace todoTask.Repositories.Tokens
{
    public class TokenRepository : ITokenRepository
    {
        // Using Iconfiguration we can access the Appsetting
        private readonly IConfiguration _configuration;

        public TokenRepository(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        public string CreateJwtToken(IdentityUser user, List<string> roles)
        {
            // Create claims
            var claims = new List<Claim>();
            // To add 
            claims.Add(new Claim(ClaimTypes.Email, user.Email));

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role,role));
            }

            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            // Hmac Sha 256 = Hash - based Message Authentication Code 
            var credentials = new SigningCredentials(Key,SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
                (
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    //Claims has Email and password roles
                    claims,
                    expires: DateTime.Now.AddMinutes(5),
                    signingCredentials: credentials
               );
            /* 
              This Basically instantiates the new jwt security token handler uses, 
              The meyhod rigth token it exposes and it takes the token and write the token
               for us
            
             */
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
