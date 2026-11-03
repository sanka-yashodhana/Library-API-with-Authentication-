using LibraryApi.Data;
using LibraryApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Repositories
{
    public class EfBookRepository : IBookRepository
    {
        private readonly AppDbContext _context;

        public EfBookRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Book book)
        {
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Book>> GetAllAsync()
        {
            return await _context.Books
                .Include(b => b.Author)
                .ToListAsync();
        }

        public async Task<Book?> GetByIdAsync(int id)
        {
            return await _context.Books
                .Include(b=> b.Author)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<bool> UpdateAsync(Book book)
        {
            var existingBook = await _context.Books.FindAsync(book.Id);

            if(existingBook is null)
            {
                return false;
            }

            existingBook.Title = book.Title;
            existingBook.AuthorId = book.AuthorId;
            existingBook.IsAvailable = book.IsAvailable;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if(book is null)
            {
                return false;
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<Book>> GetByAuthorIdAsync(int authorId)
        {
            return await _context.Books
                .Include(b => b.Author)
                .Where(b => b.AuthorId == authorId)
                .ToListAsync();
        }

        public async Task<List<Book>> GetPageAsync ( int pageNumber, int pageSize)
        {
            return await _context.Books
                .Include(b => b.Author)
                .OrderBy(b => b.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<List<Book>> GetByAvailabilityAsync(bool isAvailable)
        {
            return await _context.Books
                .Include(b => b.Author)
                .Where(b => b.IsAvailable == isAvailable)
                .ToListAsync();
        }

        public async Task<List<Book>> GetSortedAsync(string sortBy, string sortOrder)
        {
            var query = _context.Books
                .Include(b => b.Author)
                .AsQueryable();

            if(sortBy.ToLower() == "title")
            {
                query = sortOrder.ToLower() == "desc"
                    ? query.OrderByDescending(b => b.Title)
                    : query.OrderBy(b => b.Title);
            }
            else
            {
                query = sortOrder.ToLower() == "desc"
                    ? query.OrderByDescending(b => b.Id)
                    : query.OrderBy(b => b.Id);
            }
            return await query.ToListAsync();
        }

        public async Task<(List<Book> Books, int TotalCount)> SearchAsync (
            int pageNumber,
            int pageSize,
            bool? isAvailable,
            string sortBy,
            string sortOrder)
        {
            var query = _context.Books
                .Include(b => b.Author)
                .AsQueryable();

            //Filtering
            if (isAvailable.HasValue)
            {
                query = query.Where(b => b.IsAvailable == isAvailable.Value);
            }

            //Count macthing records before pagination
            var totalCount = await query.CountAsync();

            //Sorting
            if(sortBy.ToLower() == "title")
            {
                query = sortOrder.ToLower() == "desc"
                    ? query.OrderByDescending(b => b.Title)
                    : query.OrderBy(b => b.Title);
            }
            else
            {
                query = sortOrder.ToLower() == "desc"
                    ? query.OrderByDescending(b => b.Id)
                    : query.OrderBy(b => b.Id);
            }

            //Pagination
            var books = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (books, totalCount);
        }
    }
}