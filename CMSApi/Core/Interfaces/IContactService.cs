using System.Collections.Generic;
using System.Threading.Tasks;
using CMSApi.Core.DTO;

namespace CMSApi.Core.Interfaces
{
    public interface IContactService
    {
        Task<IEnumerable<ContactDTO>> GetAllContactsAsync();
        Task<ContactDTO> GetContactByIdAsync(int contactId);
        Task<ContactDTO> CreateContactAsync(ContactDTO contactDto);
        Task UpdateContactAsync(int contactId, ContactDTO contactDto);
        Task DeleteContactAsync(int contactId);
    }
}
