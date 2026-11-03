using LibraryApi.Models;
using LibraryApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApi.Services
{
    public class BookManager
    {
        private readonly IBookRepository _bookRepository;

        public BookManager(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        //Add a book
        public Task AddBookAsync(Book book)
        {
            return _bookRepository.AddAsync(book);
        }

        //Get all books
        public Task<List<Book>> GetAllBooksAsync()
        {
            return _bookRepository.GetAllAsync();
        }

        //Get a book by ID
        public Task<Book?> GetBookByIdAsync(int id)
        {
            return _bookRepository.GetByIdAsync(id);
        }

        //Update a book
        public Task<bool> UpdateBookAsync(Book book)
        {
            return _bookRepository.UpdateAsync(book);
        }

        //Delete a book
        public Task<bool> DeleteBookAsync(int id)
        {
            return _bookRepository.DeleteAsync(id);
        }

        //Get books by author ID
        public Task<List<Book>> GetBookByAuthorIdAsync(int authorId)
        {
            return _bookRepository.GetByAuthorIdAsync(authorId);
        }

        //Get books by page
        public Task<List<Book>> GetPagedBooksAsync(int pageNumber, int pageSize)
        {
            return _bookRepository.GetPageAsync(pageNumber, pageSize);
        }

        //Get books by availability
        public Task<List<Book>> GetBooksByAvailabilityAsync(bool isAvailable)
        {
            return _bookRepository.GetByAvailabilityAsync(isAvailable);
        }

        //Get books sorted by a specific field
        public Task<List<Book>> GetSortedBooksAsync(string sortBy, string sortOrder)
        {
            return _bookRepository.GetSortedAsync(sortBy, sortOrder);
        }

        //Get books by filtering, sorting and pagination
        public Task<(List<Book> Books, int TotalCount)> SearchBooksAsync(
            int pageNumber,
            int pageSize,
            bool? isAvailable,
            string sortBy,
            string sortOrder)
        {
            return _bookRepository.SearchAsync(
                pageNumber,
                pageSize,
                isAvailable,
                sortBy,
                sortOrder
                );
        }
    }
}
