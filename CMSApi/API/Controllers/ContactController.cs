using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CMSApi.Core.DTO;
using CMSApi.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;


namespace CMSApi.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllContacts()
        {
            var contacts = await _contactService.GetAllContactsAsync();
            return Ok(contacts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetContactById(int id)
        {
            var contact = await _contactService.GetContactByIdAsync(id);
            return Ok(contact);
        }

        [HttpPost]
        public async Task<IActionResult> CreateContact(ContactDTO contactDto)
        {
            var createdContact = await _contactService.CreateContactAsync(contactDto);
            return CreatedAtAction(nameof(GetContactById), new { id = createdContact.ContactId }, createdContact);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContact(int id, ContactDTO contactDto)
        {
            await _contactService.UpdateContactAsync(id, contactDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContact(int id)
        {
            await _contactService.DeleteContactAsync(id);
            return NoContent();
        }
    }
}

