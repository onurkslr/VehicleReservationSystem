namespace VehicleReservationSystem.Business.Dtos
{
    public class ReservationCreateDto
    {
        public int VehicleId { get; set; }
        public string UserName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}