using VehicleReservationSystem.Business.DTOs;
using VehicleReservationSystem.Entities;

namespace VehicleReservationSystem.Business
{
    public interface IVehicleService
    {
        Vehicle Create(VehicleCreateDto dto);
        List<Vehicle> GetAll();
        void Delete(int id);
    }
}