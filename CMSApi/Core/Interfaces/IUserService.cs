using System.Collections.Generic;
using System.Threading.Tasks;
using CMSApi.Core.DTO;

namespace CMSApi.Core.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();
        Task<UserDTO> GetUserByIdAsync(int userId);
        Task<UserDTO> CreateUserAsync(UserDTO userDto);
        Task UpdateUserAsync(int userId, UserDTO userDto);
        Task DeleteUserAsync(int userId);
    }
}
