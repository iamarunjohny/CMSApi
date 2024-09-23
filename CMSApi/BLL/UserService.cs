using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMSApi.Core.DTO;
using CMSApi.Core.Interfaces;
using CMSApi.Core.Entities;

namespace CMSApi.BLL
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(user => new UserDTO
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            });
        }

        public async Task<UserDTO> GetUserByIdAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) throw new KeyNotFoundException("User not found.");

            return new UserDTO
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }

        public async Task<UserDTO> CreateUserAsync(UserDTO userDto)
        {
            var user = new User
            {
                Username = userDto.Username,
                Passwords = userDto.Password,  // Make sure to handle password hashing properly if required
                Email = userDto.Email,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName
            };

            await _userRepository.AddAsync(user);

            return new UserDTO
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }

        public async Task UpdateUserAsync(int userId, UserDTO userDto)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) throw new KeyNotFoundException("User not found.");

            user.Username = userDto.Username;
            user.Email = userDto.Email;
            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;

            await _userRepository.UpdateAsync(user);
        }

        public async Task DeleteUserAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) throw new KeyNotFoundException("User not found.");

            await _userRepository.DeleteAsync(user);
        }
    }
}
