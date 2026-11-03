//using LibraryApi.Data;
//using LibraryApi.Models;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace LibraryApi.Controllers
//{
//    [ApiController]
//    [Route("api/efcore-test")]
//    public class EFCoreTestController : ControllerBase
//    {
//        private readonly AppDbContext _context;

//        public EFCoreTestController(AppDbContext context)
//        {
//            _context = context;
//        }

//        [HttpGet("books")]
//        public async Task<IActionResult> GetBooks()
//        {
//            var books = await _context.Books.ToListAsync();

//            return Ok(books);
//        }

//        [HttpPost("books")]
//        public async Task<IActionResult> AddBook(Book book)
//        {
//            _context.Books.Add(book);

//            await _context.SaveChangesAsync();

//            return Ok(book);
//        }

//        [HttpPut("books/{id:int}")]
//        public async Task<IActionResult> UpdateBook(int id, Book updatedBook)
//        {
//            if( id != updatedBook.Id)
//            {
//                return BadRequest("Route ID must match book ID");
//            }

//            var existingBook = await _context.Books.FindAsync(id);

//            if(existingBook is null)
//            {
//                return NotFound("Book not found");
//            }    

//            existingBook.Title = updatedBook.Title;
//            existingBook.Author = updatedBook.Author;
//            existingBook.IsAvailable = updatedBook.IsAvailable;

//            await _context.SaveChangesAsync();

//            return Ok(existingBook);
//        }
//        [HttpDelete("books/{id:int}")]
//        public async Task<IActionResult> DeleteBook(int id)
//        {
//            var book = await _context.Books.FindAsync(id);
            
//            if(book is null)
//            {
//                return NotFound("Book not found");
//            }

//            _context.Books.Remove(book);

//            await _context.SaveChangesAsync();
//            return NoContent();
//        }

//    }
//}