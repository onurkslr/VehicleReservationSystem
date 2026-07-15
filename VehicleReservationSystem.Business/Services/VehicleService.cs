using VehicleReservationSystem.Business.DTOs;
using VehicleReservationSystem.DataAccess;
using VehicleReservationSystem.Entities;

namespace VehicleReservationSystem.Business
{
    public class VehicleService : IVehicleService
    {
        private readonly AppDbContext _context;

        public VehicleService(AppDbContext context)
        {
            _context = context;
        }

        public Vehicle Create(VehicleCreateDto dto)
        {
            var vehicle = new Vehicle
            {
                Name = dto.Name,
                PlateNumber = dto.PlateNumber,
                IsDeleted = false
            };

            _context.Vehicles.Add(vehicle);
            _context.SaveChanges();

            return vehicle;
        }
        public List<Vehicle> GetAll()
        {
            var list = _context.Vehicles.ToList();

            

            return list;
        }
        public void Delete(int id)
        {
            // Fiziksel silme yerine IsDeleted flag'ini true yap
            var vehicle = _context.Vehicles.FirstOrDefault(v => v.Id == id);

            if (vehicle == null)
                throw new KeyNotFoundException("Araç bulunamadı.");

            vehicle.IsDeleted = true;
            _context.SaveChanges();
        }
    }
}