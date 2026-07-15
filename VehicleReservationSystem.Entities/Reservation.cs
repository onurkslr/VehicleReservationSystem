namespace VehicleReservationSystem.Entities;
public class Reservation{
    public int Id { get; set; }
    public int VehicleId { get; set; }
    public string UserName { get; set; }
    public DateTime StartDate { get; set; }    // Başlangıç tarihi
    public DateTime EndDate { get; set; } 
    public bool IsDeleted { get; set; }
    public Vehicle Vehicle { get; set; }
}