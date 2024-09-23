using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMSApi.Core.DTO;
using CMSApi.Core.Interfaces;
using CMSApi.Core.Entities;

namespace CMSApi.BLL
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _contactRepository;

        // Constructor to initialize the contact repository
        public ContactService(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        // Fetch all contacts and map them to ContactDTO
        public async Task<IEnumerable<ContactDTO>> GetAllContactsAsync()
        {
            var contacts = await _contactRepository.GetAllAsync();
            return contacts.Select(contact => new ContactDTO
            {
                ContactId = contact.ContactId,
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                Company = contact.Company,
                JobTitle = contact.JobTitle,
                PhoneNumber = contact.PhoneNumber,
                Email = contact.Email,
                Notes = contact.Notes,
                UserId = contact.UserId // Include UserId in the DTO
            });
        }

        // Fetch a contact by its ID and map to ContactDTO
        public async Task<ContactDTO> GetContactByIdAsync(int contactId)
        {
            var contact = await _contactRepository.GetByIdAsync(contactId);
            if (contact == null) throw new KeyNotFoundException("Contact not found.");

            return new ContactDTO
            {
                ContactId = contact.ContactId,
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                Company = contact.Company,
                JobTitle = contact.JobTitle,
                PhoneNumber = contact.PhoneNumber,
                Email = contact.Email,
                Notes = contact.Notes,
                UserId = contact.UserId // Include UserId in the DTO
            };
        }

        // Create a new contact from ContactDTO
        public async Task<ContactDTO> CreateContactAsync(ContactDTO contactDto)
        {
            var contact = new Contact
            {
                FirstName = contactDto.FirstName,
                LastName = contactDto.LastName,
                Company = contactDto.Company,
                JobTitle = contactDto.JobTitle,
                PhoneNumber = contactDto.PhoneNumber,
                Email = contactDto.Email,
                Notes = contactDto.Notes,
                UserId = contactDto.UserId // Set UserId from DTO
            };

            await _contactRepository.AddAsync(contact);

            // Return the created contact as a DTO
            return new ContactDTO
            {
                ContactId = contact.ContactId,
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                Company = contact.Company,
                JobTitle = contact.JobTitle,
                PhoneNumber = contact.PhoneNumber,
                Email = contact.Email,
                Notes = contact.Notes,
                UserId = contact.UserId  // Include UserId in the returned DTO
            };
        }

        // Update an existing contact with new values from ContactDTO
        public async Task UpdateContactAsync(int contactId, ContactDTO contactDto)
        {
            var contact = await _contactRepository.GetByIdAsync(contactId);
            if (contact == null) throw new KeyNotFoundException("Contact not found.");

            // Update contact properties
            contact.FirstName = contactDto.FirstName;
            contact.LastName = contactDto.LastName;
            contact.Company = contactDto.Company;
            contact.JobTitle = contactDto.JobTitle;
            contact.PhoneNumber = contactDto.PhoneNumber;
            contact.Email = contactDto.Email;
            contact.Notes = contactDto.Notes;
            contact.UserId = contactDto.UserId; // Update UserId


            await _contactRepository.UpdateAsync(contact);
        }

        // Delete a contact by its ID
        public async Task DeleteContactAsync(int contactId)
        {
            var contact = await _contactRepository.GetByIdAsync(contactId);
            if (contact == null) throw new KeyNotFoundException("Contact not found.");

            await _contactRepository.DeleteAsync(contact);
        }
    }
}
