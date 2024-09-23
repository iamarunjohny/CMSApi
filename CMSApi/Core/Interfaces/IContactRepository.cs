using System.Collections.Generic;
using System.Threading.Tasks;
using CMSApi.Core.Entities;

namespace CMSApi.Core.Interfaces
{
    public interface IContactRepository
    {
        Task<IEnumerable<Contact>> GetAllAsync();
        Task<Contact> GetByIdAsync(int contactId);
        Task AddAsync(Contact contact);
        Task UpdateAsync(Contact contact);
        Task DeleteAsync(Contact contact);
    }
}
