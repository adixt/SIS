﻿using System;
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
    [Route("api/SIS")]
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
        [HttpGet("getAllInsecure")]
        public IEnumerable<UsersNotSercure> GetUsersNotSercure()
        {
            return _context.UsersNotSercure;
        }

        [HttpPost("tryLoginSecure")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(bool))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> TryLoginSecure([FromBody] NamePassword user)
        {
            var actualUSerInDb = await _context.UsersSecure.Where(u => u.Name == user.Name).FirstOrDefaultAsync();

            var tryLoginHash = HashPassword(user.Password, actualUSerInDb.Salt);

            return Ok(tryLoginHash == actualUSerInDb.Password);
        }

        [HttpPost("tryLoginNotSecure")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(bool))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> TryLoginNotSecure([FromBody] NamePassword user)
        {
            var actualUSerInDb = await _context.UsersNotSercure.Where(u => u.Name == user.Name).FirstOrDefaultAsync();
            if (actualUSerInDb == null)
            {
                return Ok(string.Format("użytkownik {0} nie istnieje w naszej bazie danych", user.Name));
            }

            var isPasswordTheSame = user.Password == actualUSerInDb.Password;

            if (isPasswordTheSame)
            {
                return Ok(string.Format("zalogowano {0} hasłem {1}, SUKCES!", user.Name, user.Password));
            }
            return Ok(string.Format("dla użytkownika {0} hasło {1} nie pasuje, próbuj dalej!", user.Name, user.Password));

        }

        // GET: api/UsersNotSercures/5
        [HttpGet("getInsecureById/{id}")]
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
        [HttpGet("getSecureByName/{name}")]
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

        [HttpGet("getInsecureByName/{name}")]
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

        [HttpPut("changePasswordInsecure/{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(UsersNotSercure))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> PutUserNotSecure([FromRoute] int id, [FromBody] UsersNotSercure user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.Id)
            {
                return BadRequest();
            }

            var command = $"UPDATE UsersNotSercure SET Password = '{user.Password}' WHERE Name = '{user.Name}' ";
            var sqlConnection = new SqlConnection(_configuration["ConnectionStrings:SISContext"]);
            sqlConnection.Open();
            var sqlCommand = new SqlCommand(command, sqlConnection);
            var reader = await sqlCommand.ExecuteReaderAsync();
            return Ok("Hasło zmienione");
        }

        [HttpPut("changePassword/{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(UsersNotSercure))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> PutUserSecure([FromRoute] int id, [FromBody] UsersNotSercure user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

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

        private static string GenerateRandomSalt()
        {
            // generate a 128-bit salt using a secure PRNG
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return Convert.ToBase64String(salt);

        }

        private static string HashPassword(string password, string salt)
        {
            // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: Convert.FromBase64String(salt),
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
        }

        // POST: api/UsersNotSercures
        [HttpPost("createSecure")]
        [SwaggerResponse((int)HttpStatusCode.Created)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> PostUsersSecure([FromBody] NamePassword user)
        {
            var saltStr = GenerateRandomSalt();
            var hashed = HashPassword(user.Password, saltStr);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dbUSer = new UsersSecure
            {
                Name = user.Name,
                Password = hashed,
                Salt = saltStr
            };

            _context.UsersSecure.Add(dbUSer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsersNotSercure", new { id = dbUSer.Id }, dbUSer);
        }

        [HttpPost("createInsecure")]
        [SwaggerResponse((int)HttpStatusCode.Created)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> PostUsersNotSecure([FromBody] NamePassword user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dbUser = new UsersNotSercure
            {
                Name = user.Name,
                Password = user.Password
            };
            _context.UsersNotSercure.Add(dbUser);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsersNotSercure", new { id = dbUser.Id }, dbUser);
        }

       

        // DELETE: api/UsersNotSercures/5
        [HttpDelete("deleteInsecureById/{id}")]
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