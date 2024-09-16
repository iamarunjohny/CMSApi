using CMSApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CMSApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly MyAppDbContext _context;
        private readonly IConfiguration _config;
        public UsersController(MyAppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var users = _context.Users.ToList();
                if (users.Count == 0)
                {
                    return NotFound("Users not available.");
                }
                return Ok(users);
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
                var user = _context.Users.Find(id);
                if (user == null)
                {
                    return NotFound($"Users does not exist with id {id}");
                }
                return Ok(user);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public IActionResult Post(User user)
        {
            try
            {
                if (user == null)
                {
                    return BadRequest("User is null.");
                }

                user.UserId = 0;

                _context.Users.Add(user);
                _context.SaveChanges();

                return CreatedAtAction(nameof(Get), new { id = user.UserId }, user);
            }
            catch (DbUpdateException ex)
            {
                // Check if the exception is due to a unique constraint violation
                if (ex.InnerException != null && ex.InnerException.Message.Contains("UNIQUE"))
                {
                    return Conflict("Username already exists.");
                }
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the user.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public IActionResult Put(User user)
        {
            try
            {
                if (user == null || user.UserId == 0)
                {
                    return BadRequest("User or UserId is invalid.");
                }

                var existingUser = _context.Users.Find(user.UserId);
                if (existingUser == null)
                {
                    return NotFound($"User with ID {user.UserId} not found.");
                }

                // Update the properties
                existingUser.Username = user.Username;
                existingUser.Passwords = user.Passwords;
                existingUser.Email = user.Email;
                existingUser.FirstName = user.FirstName;
                existingUser.LastName = user.LastName;

                _context.SaveChanges();
                return Ok("User updated successfully!");
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
                var userr = _context.Users.Find(id);
                if (userr == null)
                {
                    return NotFound($"User not found with id {id}");
                }
                _context.Users.Remove(userr);
                _context.SaveChanges();
                return Ok("User deleted!");
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }




    }
}
