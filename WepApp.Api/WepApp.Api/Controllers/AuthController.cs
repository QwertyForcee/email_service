using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WepApp.Api.Auth;
using WepApp.Api.DataAccess.Repositories;
using WepApp.Api.Entities;
using WepApp.Api.Models;

namespace WepApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController:ControllerBase
    {
        private IUserRepository<User> userRep;
        private IOptions<AuthOptions> authOptions;
        public AuthController(IUserRepository<User> rep, IOptions<AuthOptions> authOptions)
        {
            this.userRep = rep;
            this.authOptions = authOptions;
        }

        [Route("login")]
        [HttpPost]
        public IActionResult Login(LoginModel credentials)
        {
            var user = this.userRep.LogIn(credentials);
            if (user != null)
            {
                var token = GenerateJWT(user);
                return new JsonResult(new { access_token = token });
            }
            return BadRequest();
        }

        [Route("register")]
        [HttpPost]
        public IActionResult Register(LoginModel credentials)
        {
            var user = this.userRep.Register(credentials);
            if (user!=null)
            {
                var token = GenerateJWT(user);
                return new JsonResult(new { access_token = token });
            }
            return BadRequest();
        }
        private string GenerateJWT(User account)
        {
            var authParams = authOptions.Value;

            var securityKey = authParams.GetSymmetricSecurityKey();
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Email,account.Email),
                new Claim(JwtRegisteredClaimNames.Sub,account.Id.ToString())
            };
            if (account.Roles != null)
            {
                foreach (var role in account.Roles)
                {
                    claims.Add(new Claim("role", role.Name));
                }

            }
            var token = new JwtSecurityToken(authParams.Issuer,
                authParams.Audience,
                claims,
                expires: DateTime.Now.AddSeconds(authParams.TokenLifetime),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
