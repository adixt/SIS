using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.Configuration;
using Remotion.Linq.Parsing.Structure;
using Swashbuckle.AspNetCore.SwaggerGen;
using WebApplication1.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WebApplication1.Controllers
{
    [Produces("application/json")]
    [Route("api/UsersNotSercures")]
    public class UsersNotSercuresController : Controller
    {
        private readonly SISContext _context;

        private IConfiguration _configuration;

        public UsersNotSercuresController(SISContext context, IConfiguration Configuration)
        {
            _context = context;
            _configuration = Configuration;
        }

        // GET: api/UsersNotSercures
        [HttpGet]
        public IEnumerable<UsersNotSercure> GetUsersNotSercure()
        {
            return _context.UsersNotSercure;
        }

        // GET: api/UsersNotSercures/5
        [HttpGet("{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(UsersNotSercure))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetUsersSercure([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var usersNotSercure = await _context.UsersNotSercure.SingleOrDefaultAsync(m => m.Id == id);

            if (usersNotSercure == null)
            {
                return NotFound();
            }

            return Ok(usersNotSercure);
        }

        // GET: api/UsersNotSercures/Name
        [HttpGet("nameSecure/{name}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(IEnumerable<UsersNotSercure>))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetUsersNotSercure([FromRoute] string name)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var usersNotSercureQ = _context.UsersNotSercure.Where(m => m.Name == name);
            //var sql = usersNotSercureQ.ToSql();
            var usersNotSercure = await usersNotSercureQ.ToListAsync();
            if (usersNotSercure == null)
            {
                return NotFound();
            }

            return Ok(usersNotSercure);
        }

        [HttpGet("nameNotSecure/{name}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(IEnumerable<UsersNotSercure>))]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetUsersSercure([FromRoute] string name)
        {
            var command = $"SELECT * FROM UsersNotSercure WHERE Name = '{name}' ";
            var sqlConnection = new SqlConnection(_configuration["ConnectionStrings:SISContext"]);
            sqlConnection.Open();
            var sqlCommand = new SqlCommand(command, sqlConnection);
            var reader = await sqlCommand.ExecuteReaderAsync();
            var users = new List<UsersNotSercure>();
            while (await reader.ReadAsync())
            {
                var x = new UsersNotSercure
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Password = reader.GetString(2)
                };
                users.Add(x);
            }

            if (users == null || users.Count == 0)
            {
                return NotFound();
            }

            return Ok(users);
        }

        // PUT: api/UsersNotSercures/5
        [HttpPut("{id}")]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        [SwaggerResponse((int)HttpStatusCode.NoContent)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> PutUsersNotSercure([FromRoute] int id, [FromBody] UsersNotSercure usersNotSercure)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != usersNotSercure.Id)
            {
                return BadRequest();
            }

            _context.Entry(usersNotSercure).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsersNotSercureExists(id))
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

        // POST: api/UsersNotSercures
        [HttpPost]
        [SwaggerResponse((int)HttpStatusCode.Created)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> PostUsersNotSercure([FromBody] UsersNotSercure usersNotSercure)
        {
            // generate a 128-bit salt using a secure PRNG
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            Console.WriteLine($"Salt: {Convert.ToBase64String(salt)}");

            // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: usersNotSercure.Password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
            Console.WriteLine($"Hashed: {hashed}");
            usersNotSercure.Password = hashed;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _context.UsersNotSercure.Add(usersNotSercure);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsersNotSercure", new { id = usersNotSercure.Id }, usersNotSercure);
        }

        // DELETE: api/UsersNotSercures/5
        [HttpDelete("{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(UsersNotSercure))]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteUsersNotSercure([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var usersNotSercure = await _context.UsersNotSercure.SingleOrDefaultAsync(m => m.Id == id);
            if (usersNotSercure == null)
            {
                return NotFound();
            }

            _context.UsersNotSercure.Remove(usersNotSercure);
            await _context.SaveChangesAsync();

            return Ok(usersNotSercure);
        }

        private bool UsersNotSercureExists(int id)
        {
            return _context.UsersNotSercure.Any(e => e.Id == id);
        }
    }
}