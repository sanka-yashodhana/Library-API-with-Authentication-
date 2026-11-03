using LibraryApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using LibraryApi.Dtos;
using LibraryApi.Services;

namespace LibraryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtTokenService _jwtTokenService;

        public AuthController(UserManager<ApplicationUser> userManager,
            JwtTokenService jwtTokenService)
        {
            _userManager = userManager;
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto request)
        {
            // Check whether the email is already registered
            var existingUser = await _userManager.FindByEmailAsync(request.Email);

            if(existingUser != null)
            {
                return BadRequest(new
                {
                    message = "Email is already registered"
                });
            }

            //Create a new user
            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email
            };

            //Identity securely hashes the password
            var result = await _userManager.CreateAsync(user, request.Password);

            if(!result.Succeeded)
            {
                return BadRequest(new
                {
                    errors = result.Errors.Select(e => e.Description)
                });
            }

            return Ok(new
            {
                message = "User registered successfully"
            });
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto request)
        {
            // Find user using email
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                return Unauthorized(new
                {
                    message = "Invalid email or password."
                });
            }

            // Check whether the password is correct
            var isPasswordValid = await _userManager.CheckPasswordAsync(
                user, request.Password);

            if (!isPasswordValid)
            {
                return Unauthorized(new
                {
                    message = "Invalid email or password."
                });
            }

            //Generate JWT token
            var token = _jwtTokenService.GenerateToken(user);

            //return token to client
            return Ok(new
            {
                message = "Login successful.",
                token = token,
                tokenType = "Bearer",
                expiresIn = 1800,
                email = user.Email
            });
        }

    }
}
