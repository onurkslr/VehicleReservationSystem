namespace VehicleReservationSystem.Entities;
public class Vehicle{
    public int Id { get; set; }
    public string Name { get; set; }
    public string PlateNumber { get; set; }
    public bool IsDeleted { get; set; }
    public ICollection<Reservation> Reservations { get; set; }
}