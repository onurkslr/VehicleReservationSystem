using VehicleReservationSystem.Business.Dtos;
using VehicleReservationSystem.Entities;

namespace VehicleReservationSystem.Business
{
    public interface IReservationService
    {
        Reservation Create(ReservationCreateDto dto);
        void Delete(int id);
    }
}