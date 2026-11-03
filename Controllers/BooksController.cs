using LibraryApi.Models;
using LibraryApi.Services;
using LibraryApi.Dtos;
using Microsoft.AspNetCore.Mvc;
using LibraryApi.Data;
using System.Linq;
using Microsoft.AspNetCore.Authorization;


namespace LibraryApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly BookManager _bookManager;
        private readonly AppDbContext _context;
        private readonly ILogger<BookController> _logger;

        public BookController(BookManager bookManager, AppDbContext context, ILogger<BookController> logger)
        {
            _bookManager = bookManager;
            _context = context;
            _logger = logger;
        }

        private static BookResponseDto ToResponseDot(Book book)
        {
            return new BookResponseDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author?.Name ?? string.Empty,
                IsAvailable = book.IsAvailable
            };
        }

        //GET: /api/books
        [HttpGet]
        public async Task<IActionResult> GetAllBooks()
        {
            var books = await _bookManager.GetAllBooksAsync();

            var response = books
                 .Select(ToResponseDot)
                 .ToList();

            return Ok(response);
        }

        //GET: /api/books/1
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetBookById(int id)
        {
            var book = await _bookManager.GetBookByIdAsync(id);

            if (book is null)
            {
                return NotFound("Book not found");
            }

            return Ok(ToResponseDot(book));
        }

        //GET: /api/author/1
        [HttpGet("author/{authorId}")]
        public async Task<IActionResult> GetBooksByAuthor(int authorId)
        {
            var authorBooks = await _bookManager.GetBookByAuthorIdAsync(authorId);

            var result = authorBooks.Select(b=> new BookResponseDto
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author?.Name ?? "Unknown",
                IsAvailable = b.IsAvailable
            })
            .ToList();

            return Ok(result);
        }

        //GET: /api/books/page?pageNumber=1&pageSize=10
        [HttpGet("paged")]
        public async Task<IActionResult> GetPagedBooks(int pageNumber = 1, int pagesize = 10)
        {
            if(pageNumber < 1 || pagesize < 1 || pagesize > 100)
            {
                return BadRequest("Invalid page number or page size. Page number must be greater than 0, and page size must be between 1 and 100.");
            }

            var books = await _bookManager.GetPagedBooksAsync(pageNumber, pagesize);

            var result = books.Select(b => new BookResponseDto
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author?.Name ?? "Unknown",
                IsAvailable = b.IsAvailable
            }).ToList();

            return Ok(result);
        }

        //Get: /api/books/availability?isAvailable=true
        [HttpGet("filter")]
        public async Task<IActionResult> GetBooksByAvailabilityAsync(bool isAvailable)
        {
            var books = await _bookManager.GetBooksByAvailabilityAsync(isAvailable);

            var result = books.Select(b => new BookResponseDto
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author?.Name ?? "Unknown",
                IsAvailable = b.IsAvailable
            }).ToList();

            return Ok(result);
        }

        //GET: /api/books/sorted?sortBy=title&sortOrder=asc
        [HttpGet("sorted")]
        public async Task<IActionResult> GetSortedBooks(string sortBy = "id", string sortOrder = "asc")
        {
            if(sortBy.ToLower() != "id" && sortBy.ToLower() != "title")
            {
                return BadRequest("Sort by must be either 'id' or 'title'");
            }

            if(sortOrder.ToLower() != "asc" && sortOrder.ToLower() != "desc")
            {
                return BadRequest("Sort order must be 'asc' or 'desc'");
            }

            var books = await _bookManager.GetSortedBooksAsync(sortBy, sortOrder);

            var result = books.Select(b => new BookResponseDto
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author?.Name ?? "Unknown",
                IsAvailable = b.IsAvailable
            }).ToList();

            return Ok(result);
        }

        //GET: /api/Book/search
        [HttpGet("search")]
        public async Task<IActionResult> SearchBooks(
    int pageNumber = 1,
    int pageSize = 10,
    bool? isAvailable = null,
    string sortBy = "id",
    string sortOrder = "asc")
        {
            if (pageNumber < 1)
            {
                return BadRequest("Page number must be at least 1.");
            }

            if (pageSize < 1 || pageSize > 100)
            {
                return BadRequest("Page size must be between 1 and 100.");
            }

            if (sortBy.ToLower() != "id" && sortBy.ToLower() != "title")
            {
                return BadRequest("Sort by must be 'id' or 'title'.");
            }

            if (sortOrder.ToLower() != "asc" && sortOrder.ToLower() != "desc")
            {
                return BadRequest("Sort order must be 'asc' or 'desc'.");
            }

            var (books, totalCount) = await _bookManager.SearchBooksAsync(
                pageNumber,
                pageSize,
                isAvailable,
                sortBy,
                sortOrder);

            var result = new PagedResultDto<BookResponseDto>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling((double)totalCount / pageSize),

                Data = books.Select(b => new BookResponseDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    Author = b.Author?.Name ?? "Unknown",
                    IsAvailable = b.IsAvailable
                }).ToList()
            };

            return Ok(result);
        }


        //POST: /api/books
        [HttpPost]
        public async Task<IActionResult> CreateBook([ FromBody] CreateBookDto dto)
        {
            _logger.LogInformation("Create book request received");

            //Check whether the author exists
            var author = await _context.Authors.FindAsync(dto.AuthorId);

            if (author is null)
            {
                _logger.LogWarning(
                    "Author with ID {AuthorId} was not found",
                    dto.AuthorId);
                return NotFound("Author not found");
            }

            //Convert the DTO int a Book model
            var book = new Book
            {
                Title = dto.Title,
                AuthorId = dto.AuthorId,
                IsAvailable = true,
            };

            await _bookManager.AddBookAsync(book);

            _logger.LogInformation(
                "Book successfully created. Book ID: {BookId}",
                book.Id);

            return CreatedAtAction(
                nameof(GetBookById),
                new { id = book.Id },
                new
                {
                    book.Id,
                    book.Title,
                    book.AuthorId,
                    book.Author?.Name,
                    book.IsAvailable
                });
        }
        //PUT: /api/book/1
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateBook(int id, UpdateBookDto dto)
        {
            var book = await _bookManager.GetBookByIdAsync(id);

            if (book == null)
            {
                return NotFound("Book not found");
            }

            var author = await _context.Authors.FindAsync(dto.AuthorId);

            if(author == null)
            {
                return NotFound("Author not found");
            }

            book.Title = dto.Title;
            book.AuthorId = dto.AuthorId;
            book.IsAvailable = dto.IsAvailable;

            await _bookManager.UpdateBookAsync(book);

            return Ok("Book updated successfully");
        }

        //PATCH: /api/books/1/availability?isAvailable=true
        [HttpPatch("{id}/availability")]
        public async Task<IActionResult> UpdateBookAvailability(int id, bool isAvailable)
        {
            var book = await _bookManager.GetBookByIdAsync(id);

            if(book == null)
            {
                return NotFound("Book not found");
            }
            book.IsAvailable = isAvailable;
            await _bookManager.UpdateBookAsync(book);
            return Ok("Book availability updated successfully");
        }

        //DELETE /api/books/1
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            bool deleted = await _bookManager.DeleteBookAsync(id);

            if (!deleted)
                return NotFound("Book not found");

            return NoContent();
        }

    }
}