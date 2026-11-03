using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace LibraryApi.Models
{
    public class Book
    {
        [Range(1,int.MaxValue, ErrorMessage ="Book ID must be positive")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Book title is required.")]
        [StringLength(150, 
            MinimumLength = 2,
            ErrorMessage = "Title must be between 2 and 150 characters.")]
        public string Title { get; set; } = string.Empty;

        [Range(1, int.MaxValue,
            ErrorMessage = "Please Provide a valid Author ID")]
        public int? AuthorId { get; set; }

        public Author? Author { get; set; }

        public bool IsAvailable { get; set; } = true;
    }
}
