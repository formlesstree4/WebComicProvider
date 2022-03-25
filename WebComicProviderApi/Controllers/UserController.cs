﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebComicProvider.Interfaces;
using WebComicProvider.Models.User;

namespace WebComicProviderApi.Controllers
{

    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly IUserManager userManager;
        private readonly IUserTokenManager userTokenManager;

        public UserController(IConfiguration configuration, IUserManager userManager, IUserTokenManager userTokenManager)
        {
            this.configuration = configuration;
            this.userManager = userManager;
            this.userTokenManager = userTokenManager;
        }


        

        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody] UserLoginRequest loginRequest)
        {
            var authentication = await userManager.Authenticate(loginRequest);
            if (!authentication.Item1 || authentication.Item2 is null)
            {
                return Unauthorized();
            }

            var token = CreateToken(loginRequest.UserName, authentication.Item2.Email, authentication.Item2.UserRoles);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            authentication.Item2.Token = tokenString;
            await userTokenManager.CreateSession(authentication.Item2.Username, authentication.Item2);

            return Ok(new
            {
                token = tokenString
            });
        }

        [HttpPut, AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] UserRegisterRequest registerRequest)
        {
            var result = await userManager.RegisterUser(registerRequest);
            return result.Success ? Ok(result) : BadRequest(result.ErrorMessage);
        }

        [HttpGet]
        public async Task<IActionResult> Refresh()
        {
            var username = User.GetUsername();
            var email = User.GetEmail();

            if (username is null || email is null) return BadRequest();
            var session = await userTokenManager.GetSession(username);
            if (session is null) return Unauthorized();

            var token = CreateToken(username, email, session.UserRoles);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            await userTokenManager.UpdateSessionToken(username, tokenString);
            return Ok(new
            {
                token = tokenString
            });
        }

        [HttpDelete]
        public async Task<IActionResult> Logout()
        {
            await userTokenManager.ExpireSession(User.GetUsername());
            return Ok();
        }


        private JwtSecurityToken CreateToken(string username, string email, IEnumerable<string> roles)
        {
            var claims = new[]
            {
                new Claim("Username", username),
                new Claim("Email", email),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, string.Join(',', roles)),
                new Claim(ClaimTypes.Authentication, "true")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            return new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);
        }


    }
}
