using System.Collections.Generic;
using JokeWebApp.Data;
using JokeWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JokeWebApp.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class JokesApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public JokesApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/JokesApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Joke>>> GetJokes()
        {
            var jokes = await _context.Joke.AsNoTracking().ToListAsync();
            return Ok(jokes);
        }

        // GET: api/JokesApi/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Joke>> GetJoke(int id)
        {
            var joke = await _context.Joke.AsNoTracking().FirstOrDefaultAsync(j => j.Id == id);

            if (joke == null)
            {
                return NotFound();
            }

            return Ok(joke);
        }

        // POST: api/JokesApi
        [HttpPost]
        public async Task<ActionResult<Joke>> CreateJoke(Joke joke)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            _context.Joke.Add(joke);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetJoke), new { id = joke.Id }, joke);
        }

        // PUT: api/JokesApi/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateJoke(int id, Joke joke)
        {
            if (id != joke.Id)
            {
                return BadRequest("Route id and payload id do not match.");
            }

            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var exists = await _context.Joke.AnyAsync(j => j.Id == id);
            if (!exists)
            {
                return NotFound();
            }

            _context.Entry(joke).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/JokesApi/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteJoke(int id)
        {
            var joke = await _context.Joke.FindAsync(id);
            if (joke == null)
            {
                return NotFound();
            }

            _context.Joke.Remove(joke);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
