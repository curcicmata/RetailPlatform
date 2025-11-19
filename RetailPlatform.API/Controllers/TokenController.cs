using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace RetailPlatform.API.Controllers
{
    [ApiController]
    [Route("api/token")]
    public class TokenController(IConfiguration configuration) : ControllerBase
    {
        [HttpPost]
        public IActionResult CreateToken([FromBody] LoginRequest loginRequest)
        {
            if (loginRequest.Username != "test" || loginRequest.Password != "Password123!")
            {
                return Unauthorized();
            }

            var claims = new[]
            {
                new Claim("id", Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, loginRequest.Username)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );

            return Ok(new
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }
    }

    public record LoginRequest(string Username, string Password);
}
