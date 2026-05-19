using Backend.DTOs.Request;
using Backend.DTOs.Response;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly AuthService _authService;

        public AuthController(ILogger<AuthController> logger, AuthService authService)
        {
            _logger = logger;
            _authService = authService;
        }
        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<String>>> Login([FromBody] LoginRequest request)
        {
            var token = await _authService.CheckLogin(request);

            if (token == null) {
                return Unauthorized();
            }
            return Ok(new ApiResponse<String>
            {
                Code = 200,
                Message = "Login successful",
                Results = token
            });
        }
    }
}
