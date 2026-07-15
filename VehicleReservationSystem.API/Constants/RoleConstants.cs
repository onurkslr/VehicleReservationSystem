namespace VehicleReservationSystem.API.Constants
{
    // Bu sınıf sadece controller attribute'larında kullanmak için
    // string literal yazmayı önler. Gerçek roller appsettings.json'da.
    public static class RoleConstants
    {
        public const string Admin = "Admin";
        public const string VehicleManager = "VehicleManager";
        public const string ReservationUser = "ReservationUser";
    }
}