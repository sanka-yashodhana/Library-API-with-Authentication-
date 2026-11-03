
using System.ComponentModel.DataAnnotations;

namespace LibraryApi.Dtos
{
    public class CreateBookDto
    {
        //[Range(1, int.MaxValue, ErrorMessage = "Book ID must be positive.")]
        //public int Id { get; set; }

        [Required(ErrorMessage = "Book title is required.")]
        [StringLength(150, MinimumLength = 2, ErrorMessage = "Title must be between 2 and 150 characters.")]
        public string Title { get; set; } = string.Empty;

        //[Required(ErrorMessage = "Author name is required.")]
        //[StringLength(100, ErrorMessage = "Author cannot exceed 100 characters.")]
        //public string Author { get; set; } = string.Empty;

        [Required(ErrorMessage = "Author ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Author ID must be positive.")]
        public int? AuthorId { get; set; }

        //public bool IsAvailable { get; set; } = true;
    }
}
