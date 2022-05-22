using Microsoft.AspNetCore.Authorization;
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
    public sealed class UserController : WebComicProviderApiControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly IUserManager userManager;
        private readonly IUserTokenManager userTokenManager;
        private readonly ILogger<UserController> logger;

        public UserController(IConfiguration configuration, IUserManager userManager, IUserTokenManager userTokenManager, ILogger<UserController> logger)
        {
            this.configuration = configuration;
            this.userManager = userManager;
            this.userTokenManager = userTokenManager;
            this.logger = logger;
        }



        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody] UserLoginRequest loginRequest)
        {
            var authentication = await userManager.Authenticate(loginRequest);
            if (!authentication.Item1 || authentication.Item2 is null)
            {
                return Unauthorized();
            }
            var token = CreateToken(loginRequest.UserName, authentication.Item2.Email, authentication.Item2.UserId, authentication.Item2.UserRoles);
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
        public async Task<IActionResult> Profile()
        {
            var username = User.GetUsername();
            if (username is null) return BadRequest();
            return Ok(await userTokenManager.GetSession(username));
        }

        [HttpPut]
        public IActionResult Profile([FromBody] UpdateUserProfileRequest profile)
        {
            return NotImplemented();
        }

        [HttpGet]
        public async Task<IActionResult> Refresh()
        {
            var username = User.GetUsername();
            var email = User.GetEmail();
            var userId = User.GetUserId();

            if (username is null || email is null || userId is null) return BadRequest();
            var session = await userTokenManager.GetSession(username);
            if (session is null) return Unauthorized();

            var token = CreateToken(username, email, userId.Value, session.UserRoles);
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
            var username = User.GetUsername();
            if (username is null)
            {
                return BadRequest();
            }
            await userTokenManager.ExpireSession(username);
            return Ok();
        }


        private JwtSecurityToken CreateToken(string username, string email, int userId, IEnumerable<string> roles)
        {
            var claims = new[]
            {
                new Claim("Username", username),
                new Claim("Email", email),
                new Claim("UserId", userId.ToString()),
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
