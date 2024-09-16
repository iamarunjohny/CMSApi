using CMSApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace CMSApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly MyAppDbContext _context;
        private readonly IConfiguration _config;
        public ContactsController(MyAppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var contacts = _context.Contacts.ToList();
                if (contacts.Count == 0)
                {
                    return NotFound("Users not available.");
                }
                return Ok(contacts);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var contacts = _context.Contacts.Find(id);
                if (contacts == null)
                {
                    return NotFound($"Contact does not exist with id {id}");
                }
                return Ok(contacts);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public IActionResult Post(Contact contact)
        {
            try
            {
                if (contact == null)
                {
                    return BadRequest("Contact is null.");
                }

                contact.ContactId = 0;

                _context.Contacts.Add(contact);
                _context.SaveChanges();

                return CreatedAtAction(nameof(Get), new { id = contact.ContactId }, contact);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public IActionResult Put(Contact contact)
        {
            try
            {
                if (contact == null || contact.UserId == 0)
                {
                    return BadRequest("Contact or ContactID is invalid.");
                }

                var existingContact = _context.Contacts.Find(contact.ContactId);
                if (existingContact == null)
                {
                    return NotFound($"User with ID {contact.ContactId} not found.");
                }

                // Update the properties
                existingContact.FirstName = contact.FirstName;
                existingContact.LastName = contact.LastName;
                existingContact.Company = contact.Company;
                existingContact.JobTitle = contact.JobTitle;
                existingContact.PhoneNumber = contact.PhoneNumber;
                existingContact.Email = contact.Email;
                existingContact.Notes = contact.Notes;

                _context.SaveChanges();
                return Ok("Contact updated successfully!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                var contact = _context.Contacts.Find(id);
                if (contact == null)
                {
                    return NotFound($"Contact not found with id {id}");
                }
                _context.Contacts.Remove(contact);
                _context.SaveChanges();
                return Ok("Contact deleted!");
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }




    }
}
