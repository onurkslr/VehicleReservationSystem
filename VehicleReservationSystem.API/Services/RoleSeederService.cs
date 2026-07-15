using Microsoft.AspNetCore.Identity;

namespace VehicleReservationSystem.API.Services
{
    public class RoleSeederService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;

        public RoleSeederService(
            RoleManager<IdentityRole> roleManager,
            UserManager<IdentityUser> userManager,
            IConfiguration configuration)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task SeedRolesAsync()
        {
            // 1. appsettings.json'dan rolleri oku ve yoksa oluştur
            var roles = _configuration.GetSection("Roles").Get<List<string>>();
            if (roles == null) return;

            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                    await _roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        public async Task SeedAdminUserAsync()
        {
            var adminSettings = _configuration.GetSection("AdminUser");
            var email = adminSettings["Email"];
            var userName = adminSettings["UserName"];
            var password = adminSettings["Password"];

            // 2. Admin kullanıcı zaten var mı kontrol et
            var existingAdmin = await _userManager.FindByEmailAsync(email);
            if (existingAdmin != null) return;

            // 3. Admin kullanıcıyı oluştur
            var adminUser = new IdentityUser
            {
                UserName = userName,
                Email = email
            };

            var result = await _userManager.CreateAsync(adminUser, password);
            if (!result.Succeeded) return;

            // 4. Admin rolünü ata
            await _userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}