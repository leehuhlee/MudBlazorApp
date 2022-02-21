using MudBlazorApp.Shared.Models;

namespace MudBlazorApp.Server.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly DataContext _context;

        public UserService(DataContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<List<User>>> GetUsersAsync()
        {
            var allUser = await _context.Users.ToListAsync();
            return new ServiceResponse<List<User>> { Data = allUser };
        }

        public async Task<ServiceResponse<User>> GetUserDetailsAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            return new ServiceResponse<User> { Data = user };
        }

        public async Task<ServiceResponse<string>> GetUserProfilePictureAsync(int userId)
        {
            var findUser = await _context.Users.FindAsync(userId);
            return new ServiceResponse<string> { Data = findUser.ProfilePictureDataUrl };
        }

        public async Task<ServiceResponse<bool>> UpdateUserAsync(User user)
        {
            var findUser = await _context.Users.FindAsync(user.Id);
            findUser.FirstName = user.FirstName;
            findUser.LastName = user.LastName;
            findUser.Email = user.Email;
            await _context.SaveChangesAsync();
            return new ServiceResponse<bool> { Message = "Update User Successful!", Data = true };
        }

        public async Task<ServiceResponse<bool>> DeleteUserAsync(int userId)
        {
            var findUser = await _context.Users.FindAsync(userId);
            _context.Users.Remove(findUser);
            await _context.SaveChangesAsync();
            return new ServiceResponse<bool> { Message = "Delete User Successful!", Data = true };
        }
    }
}
