using LibraryApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApi.Repositories
{
    public interface IBookRepository
    {
        Task AddAsync(Book book);

        Task<List<Book>> GetAllAsync();

        Task<Book?> GetByIdAsync(int id);

        Task<bool> UpdateAsync(Book book);

        Task<bool> DeleteAsync(int id);

        Task<List<Book>> GetByAuthorIdAsync(int authorId);

        Task<List<Book>> GetPageAsync(int pageNumber, int pageSize);

        Task<List<Book>> GetByAvailabilityAsync(bool isAvailable);

        Task<List<Book>> GetSortedAsync(string sortBy, string sortOrder);

        Task<(List<Book> Books, int TotalCount)> SearchAsync(
            int pageNumber,
            int pageSize,
            bool? isAvailable,
            string sortBy,
            string sortOrder);
    }
}
