using System.Collections.Generic;
using System.Threading.Tasks;
using CMSApi.Core.Entities;

namespace CMSApi.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetByIdAsync(int userId);
        Task<User> GetByUsernameAsync(string username);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(User user);
    }
}
