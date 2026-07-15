using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VehicleReservationSystem.Business.Dtos;

namespace VehicleReservationSystem.Business
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<string> Register(RegisterDto dto)
{
    // 1. appsettings.json'dan geçerli rolleri oku
    var validRoles = _configuration.GetSection("Roles").Get<List<string>>();

    // 2. Gelen rol listede var mı kontrol et
    if (validRoles == null || !validRoles.Contains(dto.Role))
        throw new InvalidOperationException(
            $"Geçersiz rol: {dto.Role}. Geçerli roller: {string.Join(", ", validRoles)}");

    // 3. Email daha önce kayıtlı mı kontrol et
    var existingUser = await _userManager.FindByEmailAsync(dto.Email);
    if (existingUser != null)
        throw new InvalidOperationException("Bu email zaten kayıtlı.");

    // 4. Yeni kullanıcı oluştur
    var user = new IdentityUser
    {
        UserName = dto.UserName,
        Email = dto.Email
    };

    // 5. Kullanıcıyı kaydet (şifre otomatik hashlenir)
    var result = await _userManager.CreateAsync(user, dto.Password);

    if (!result.Succeeded)
    {
        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
        throw new InvalidOperationException(errors);
    }

    // 6. Rolü ata (RoleSeeder sayesinde rol zaten mevcut)
    await _userManager.AddToRoleAsync(user, dto.Role);

    // 7. Token üret ve döndür
    return await GenerateToken(user);
}

        public async Task<string> Login(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
                throw new UnauthorizedAccessException("Email veya şifre hatalı.");

            return await GenerateToken(user);
        }

        private async Task<string> GenerateToken(IdentityUser user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];
            var expiration = int.Parse(jwtSettings["ExpirationInMinutes"]);

            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            // Rolleri claim olarak ekle
            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiration),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}