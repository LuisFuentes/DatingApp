using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.DataTransferObjects;
using DatingApp.API.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]

    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;

        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserRegistrationDto userRegistration)
        {
            // Function attempts to register the username

            // TODO: Validate request

            userRegistration.Username = userRegistration.Username.ToLower();

            if (await _repo.UserExists(userRegistration.Username))
                return BadRequest("Username already exists");

            User newUser = new User
            {
                Username = userRegistration.Username
            };

            await _repo.Register(newUser, userRegistration.Password);
            return StatusCode(201);

            // TODO: Send back the route for the new user
            //return CreatedAtRoute("", await _repo.Register(newUser, password));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto userLogin)
        {
            // Function attempts to log in the user account

            User userAccount = await _repo.Login(userLogin.Username, userLogin.Password);

            if (userAccount == null)
                return Unauthorized();

            // Fetch a JWT Token for this user account
            return Ok(new { 
                token = GenerateJwtToken(userAccount.Id.ToString(), userAccount.Username)
            });
        }

        private string GenerateJwtToken(string userId, string username)
        {
            // Function handles create the JWT token based on the provided
            // user ID and username

            // Generate the user claims by User Id & Name
            Claim[] jwtClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Name, username)
            };

            // Generate the Key, using the App token
            SymmetricSecurityKey jwtKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

            // Generate the creds
            SigningCredentials credentials = new SigningCredentials(jwtKey, SecurityAlgorithms.HmacSha512Signature);

            // Create a  JWT Desc object
            SecurityTokenDescriptor jwtDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(jwtClaims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials
            };

            System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler jwtHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();

            // Finally create the JWT token & return it
            SecurityToken jwtToken = jwtHandler.CreateToken(jwtDescriptor);

            return jwtHandler.WriteToken(jwtToken);
        }
    }
}