using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Issuer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration config;

        public AuthController(IConfiguration config)
        {
            this.config = config;
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var userName = "radek";
            var pass = "radek";

            if (userName == username && pass == password)
            {
                var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim("role", "user")
                // Add more claims as needed
            };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:secret"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                        issuer: config["JWT:issuer"],
                        claims: claims,
                        expires: DateTime.UtcNow.AddMinutes(20),
                        signingCredentials: creds,
                        audience: config["JWT:audience"]
                    );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token)
                });
            }

            return Unauthorized();
        }
    }
}
