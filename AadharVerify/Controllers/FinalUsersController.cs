using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AadharVerify.Models;
using AadharVerify.Dto;

namespace AadharVerify.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinalUsersController : ControllerBase
    {
        private readonly UserDataDbContext _context;

        public FinalUsersController(UserDataDbContext context)
        {
            _context = context;
        }

        // GET: api/FinalUsers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Users>>> GetUsersList()
        {
          if (_context.UsersList == null)
          {
              return NotFound();
          }
            return await _context.UsersList.ToListAsync();
        }

        // GET: api/FinalUsers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Users>> GetUsers(int id)
        {
          if (_context.UsersList == null)
          {
              return NotFound();
          }
            var users = await _context.UsersList.FindAsync(id);

            if (users == null)
            {
                return NotFound();
            }

            return users;
        }

        // PUT: api/FinalUsers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsers(int id, Users users)
        {
            if (id != users.Id)
            {
                return BadRequest();
            }

            _context.Entry(users).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsersExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/FinalUsers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Users>> PostUsers(Users users)
        {
          if (_context.UsersList == null)
          {
              return Problem("Entity set 'UserDataDbContext.UsersList'  is null.");
                
            }
            users.UserType = "user";

            _context.UsersList.Add(users);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsers", new { id = users.Id }, users);
        }
        [HttpGet("email-exists/{email}")]
        public IActionResult CheckEmailExists(string email)
        {
            var exists = _context.UsersList.Any(u => u.Email == email);

            if (exists)
            {
                return Ok(new { exists = true, message = "Email already registered." });
            }
            else
            {
                return Ok(new { exists = false, message = "Email is unique." });
            }
        }


        // DELETE: api/FinalUsers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsers(int id)
        {
            if (_context.UsersList == null)
            {
                return NotFound();
            }
            var users = await _context.UsersList.FindAsync(id);
            if (users == null)
            {
                return NotFound();
            }

            _context.UsersList.Remove(users);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsersExists(int id)
        {
            return (_context.UsersList?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
