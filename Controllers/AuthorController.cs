using LibraryApi.Data;
using LibraryApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryApi.Dtos;

namespace LibraryApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthorController(AppDbContext context)
        {
            _context = context;
        }

        //Get: api/author
        [HttpGet]
        public async Task<IActionResult> GetAllAuthors()
        {
            var authors = await _context.Authors.ToListAsync();

            return Ok(authors);
        }

        //Get: api/author/1
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAuthorById(int id)
        {
            var author = await _context.Authors.FirstOrDefaultAsync(a=> a.Id == id);

            if (author is null)
            {
                return NotFound("Author not found");
            }

            return Ok(author);
        }

        //Post: api/author
        [HttpPost]
        public async Task<IActionResult> CreateAuthor(CreateAuthorDto dto)
        {
            var author = new Author
            {
                Name = dto.Name
            };

            _context.Authors.Add(author);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetAuthorById),
                new {id = author.Id},
                author
            );
        }

        //Put: api/author/1
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateAuthor(int id, Author author)
        {
            if(id != author.Id)
            {
                return BadRequest("Author ID missmatch");
            }

            var existingAuthor = await _context.Authors.FindAsync(id);

            if(existingAuthor is null)
            {
                return NotFound("Author not found");
            }

            existingAuthor.Name = author.Name;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        //Delete: api/author/1
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var author = await _context.Authors.FindAsync(id);

            if(author is null)
            {
                return NotFound("Author not found");
            }

            _context.Authors.Remove(author);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
