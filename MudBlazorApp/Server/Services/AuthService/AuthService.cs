using Microsoft.IdentityModel.Tokens;
using MudBlazorApp.Server.Services.UploadService;
using MudBlazorApp.Shared.Models;
using MudBlazorApp.Shared.Requests;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace MudBlazorApp.Server.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly DataContext _context;
        private readonly IUploadService _uploadService;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(DataContext context,
            IUploadService uploadService,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _uploadService = uploadService;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        public string GetUserEmail() => _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
        public async Task<bool> UserExists(string email)
        {
            if (await _context.Users.AnyAsync(user => user.Email.ToLower()
                 .Equals(email.ToLower())))
            {
                return true;
            }
            return false;
        }

        public async Task<ServiceResponse<User>> GetUserAsync()
        {
            var userId = GetUserId();
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            return new ServiceResponse<User>
            {
                Data = user
            };
        }

        public async Task<ServiceResponse<string>> GetProfilePictureAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return new ServiceResponse<string> { Data = null, Message = "User Not Found" };
            return new ServiceResponse<string> { Data = user.ProfilePictureDataUrl };
        }

        public async Task<ServiceResponse<int>> Register(User user, string password)
        {
            if (await UserExists(user.Email))
            {
                return new ServiceResponse<int>
                {
                    Success = false,
                    Message = "User already exists."
                };
            }

            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new ServiceResponse<int> { Data = user.Id, Message = "Registration successful!" };
        }

        public async Task<ServiceResponse<string>> Login(string email, string password)
        {
            var response = new ServiceResponse<string>();
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Email.ToLower().Equals(email.ToLower()));
            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found.";
            }
            else if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                response.Success = false;
                response.Message = "Wrong password.";
            }
            else
            {
                user.IsOnline = true;
                await _context.SaveChangesAsync();
                response.Data = CreateToken(user);
                response.Message = $"Welcome {user.GetUserName()}!";
            }

            return response;
        }

        public async Task<ServiceResponse<bool>> Logout(int userId)
        {
            var findUser = await _context.Users.FindAsync(userId);
            findUser.IsOnline = false;
            await _context.SaveChangesAsync();
            return new ServiceResponse<bool> { Data = true, Message = "Logout Successful!" };
        }

        public async Task<ServiceResponse<bool>> ChangePassword(int userId, string password, string newPassword)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return new ServiceResponse<bool>
                {
                    Success = false,
                    Message = "User not found."
                };
            }
            else if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return new ServiceResponse<bool>
                {
                    Success = false,
                    Message = "Wrong password."
                };
            }
            else
            {
                CreatePasswordHash(newPassword, out byte[] passwordHash, out byte[] passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;

                await _context.SaveChangesAsync();

                return new ServiceResponse<bool> { Data = true, Message = "Password has been changed." };
            }
        }

        public async Task<ServiceResponse<string>> UpdateUserDetailsAsync(User user)
        {
            var findUser = await _context.Users.FindAsync(user.Id);
            findUser.FirstName = user.FirstName;
            findUser.LastName = user.LastName;
            await _context.SaveChangesAsync();
            return new ServiceResponse<string> { Data = CreateToken(findUser), Message = "Update Profile Successful!" };
        }

        public async Task<ServiceResponse<bool>> UpdateProfilePictureAsync(UpdateProfilePictureRequest request, int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return new ServiceResponse<bool> { Data = false, Message = "User Not Found" };
            var filePath = _uploadService.UploadAsync(request);
            user.ProfilePictureDataUrl = filePath;
            await _context.SaveChangesAsync();
            return new ServiceResponse<bool> { Data = true, Message = "Update Profile Picture Successful!" };
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac
                    .ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash =
                    hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        private Role GetUserRole(User user)
        {
            var userRole = _context.Roles.Where(a => a.Id == user.RoleId).FirstOrDefault();
            return userRole;
        }

        private string CreateToken(User user)
        {
            var userRole = GetUserRole(user);
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.GetUserName()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, userRole.Name)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8
                .GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email.Equals(email));
        }
    }
}
