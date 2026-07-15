using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleReservationSystem.API.Constants;
using VehicleReservationSystem.Business;
using VehicleReservationSystem.Business.DTOs;
using VehicleReservationSystem.Entities;

namespace VehicleReservationSystem.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;

        public VehicleController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }
        [Authorize(Roles = $"{RoleConstants.Admin},{RoleConstants.VehicleManager}")]
        [HttpPost]
        public IActionResult Create([FromBody] VehicleCreateDto dto)
        {
            var vehicle = _vehicleService.Create(dto);
            return Ok(vehicle);
        }
        [Authorize(Roles = $"{RoleConstants.Admin},{RoleConstants.VehicleManager}")]
        [HttpGet]
        public IActionResult Get()
        {
           var list = _vehicleService.GetAll();
           return Ok(list);
        }
    }
}