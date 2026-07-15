using VehicleReservationSystem.Business.Dtos;
using VehicleReservationSystem.DataAccess;
using VehicleReservationSystem.Entities;

namespace VehicleReservationSystem.Business
{
    public class ReservationService : IReservationService
    {
        private readonly AppDbContext _context;

        public ReservationService(AppDbContext context)
        {
            _context = context;
        }

        public Reservation Create(ReservationCreateDto dto)
        {
    var conflict = _context.Reservations.Any(r =>
        r.VehicleId == dto.VehicleId &&
        r.IsDeleted == false &&
        r.StartDate < dto.EndDate &&
        r.EndDate > dto.StartDate
    );

    if (conflict)
    {
        throw new InvalidOperationException(
            "Bu araç seçilen tarih aralığında başkası tarafından rezerve edilmiştir."
        );
    }

    var reservation = new Reservation
    {
        VehicleId = dto.VehicleId,
        UserName = dto.UserName,
        StartDate = dto.StartDate,
        EndDate = dto.EndDate,
        IsDeleted = false
    };

    _context.Reservations.Add(reservation);
    _context.SaveChanges();

    return reservation;
        }
        public void Delete(int id)
            {
                var reservation = _context.Reservations.FirstOrDefault(r => r.Id == id);

                if (reservation == null)
                    throw new KeyNotFoundException("Rezervasyon bulunamadı.");

                reservation.IsDeleted = true;
                _context.SaveChanges();
            }
    }
    
    
}