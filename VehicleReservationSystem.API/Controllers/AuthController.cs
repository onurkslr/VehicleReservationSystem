using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleReservationSystem.API.Constants;
using VehicleReservationSystem.Business;
using VehicleReservationSystem.Business.Dtos;

namespace VehicleReservationSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // Sadece Admin yeni kullanıcı kaydedebilir
        [Authorize(Roles = RoleConstants.Admin)]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var token = await _authService.Register(dto);
            return Ok(new { token });
        }

        // Login herkese açık
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var token = await _authService.Login(dto);
            return Ok(new { token });
        }
    }
}