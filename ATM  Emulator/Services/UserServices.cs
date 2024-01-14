using ATM__Emulator.Dtos;
using ATM__Emulator.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ATM__Emulator.Services
{
    public class UserServices : IUserServices
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;


        public UserServices(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<User> Signup(UserRequestDto userData)
        {
            string hashedPassword, salt="";
            hashedPassword = HashPassword(userData.Password,ref salt);

            var user = new User
            {
                UserName = userData.UserName,
                Balance = 0,
                Password = hashedPassword,
                Salt = salt
            };

            await _context.AddAsync(user);

            _context.SaveChanges();

            return user;
        }

        public async Task<LoginResponseDto> Login(UserRequestDto userData)
        {
            var user =  _context.Users.SingleOrDefault(u => u.UserName == userData.UserName);

            if (user == null)
            {
                throw new Exception("Wrong Name or Password");
            }
            
            if (VerifyPassword(user.Password,user.Salt,userData.Password)) 
            {
                var token = GenerateJWT(user);
                return new LoginResponseDto { 
                    Id =user.Id,
                    UserName = user.UserName,
                    Balance = user.Balance,
                    AccessToken = token};
            }

            throw new Exception("Wrong Name or Password");
        }

        private string HashPassword(string password, ref string salt)
        {
            byte[] saltBytes = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            salt = Convert.ToBase64String(saltBytes);

            // Hash the password with the salt
            string hashedPassword = Convert.ToBase64String(
                KeyDerivation.Pbkdf2(
                    password: password,
                    salt: saltBytes,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8
                )
            );

            return hashedPassword;
        }

        private bool VerifyPassword(string hashedPassword, string salt, string providedPassword)
        {
            // Verify the provided password against the hashed password and salt
            string hashedProvidedPassword = Convert.ToBase64String(
                KeyDerivation.Pbkdf2(
                    password: providedPassword,
                    salt: Convert.FromBase64String(salt),
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8
                )
            );

            return hashedPassword == hashedProvidedPassword;
        }
        
        private string GenerateJWT(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var issuer = _config["Jwt:Issuer"];
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim("UserId", user.Id.ToString()),
            };

            var Sectoken = new JwtSecurityToken(issuer,
              issuer,
              claims,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);
            return token;
        }
        
        public bool UserExist(string userName)
        {
            return _context.Users.Any(x => x.UserName == userName);
        }
    }
}
