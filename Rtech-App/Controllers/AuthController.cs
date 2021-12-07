using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Rtech_App.DTOs;
using Rtech_App.Interface;
using Rtech_App.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Rtech_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _Config;
        public AuthController(IAuthRepository repo, IConfiguration Config)
        {
            _repo = repo;
            _Config = Config;
        }

        //EndDay

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody]UserForRegisterDto userForRegisterDto)
        {
      
            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();

            if (await _repo.UserExists(userForRegisterDto.Username))
                return BadRequest("UserName already Exists");

            var userToCreate = new User
            {
                UserName = userForRegisterDto.Username
            };

            var CreatedUser = await _repo.Register(userToCreate, userForRegisterDto.Password);

            return StatusCode(201);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            var userFromrepo = await _repo.Login(userForLoginDto.Username.ToLower(), userForLoginDto.Password);

            if (userFromrepo == null)
                return Unauthorized();

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userFromrepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromrepo.UserName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(_Config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new
            {
                token = tokenHandler.WriteToken(token)

            });
        }
    }
}
