using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Persistence.Context;
using WebApplication2.Persistence.Models;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UsersContext _context;

        public UsersController(UsersContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Users>>> GetUsers(CancellationToken cancellationToken,[FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? search = null)
        {
            IQueryable<Users> query = _context.Users;

            // Filtering based on search (on FirstName, LastName, EmailAddress)
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(
                    u => u.FirstName.Contains(search) ||
                                    u.LastName.Contains(search) ||
                                    u.EmailAddress.Contains(search) ||
                                    u.PhoneNumber.Contains(search));
            }

            // Total count before paging
            int totalCount = await query.CountAsync(cancellationToken);

            // Paging logic
            query = query.Skip((pageNumber - 1) * pageSize)
                         .Take(pageSize);

            // Fetch the result
            var users = await query.ToListAsync(cancellationToken);

            // Response with metadata for pagination
            var response = new
            {
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / pageSize),
                Data = users
            };

            return Ok(response);
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Users>> GetUsers(int id)
        {
            var users = await _context.Users.FindAsync(id);

            if (users == null)
            {
                return NotFound();
            }

            return users;
        }

        // PUT: api/Users/5
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

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Users>> PostUsers(Users users)
        {
            _context.Users.Add(users);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsers", new { id = users.Id }, users);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsers(int id)
        {
            var users = await _context.Users.FindAsync(id);
            if (users == null)
            {
                return NotFound();
            }

            _context.Users.Remove(users);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsersExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
