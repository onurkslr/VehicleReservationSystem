using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleReservationSystem.API.Constants;
using VehicleReservationSystem.Business;
using VehicleReservationSystem.Business.Dtos;
using VehicleReservationSystem.Business.DTOs;
using VehicleReservationSystem.Entities;

namespace VehicleReservationSystem.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }
        [Authorize(Roles = $"{RoleConstants.Admin},{RoleConstants.ReservationUser}")]
        [HttpPost]
        public IActionResult Create([FromBody] ReservationCreateDto dto)
        {
            
                var reservation = _reservationService.Create(dto);
                return Ok(reservation);
            
        }
    }
}