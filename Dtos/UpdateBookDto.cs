using System.ComponentModel.DataAnnotations;

namespace LibraryApi.Dtos
{
    public class UpdateBookDto
    {
        [Required(ErrorMessage = "Book tiyte is required.")]
        [StringLength(150, MinimumLength = 2, ErrorMessage = "Title must be between 2 and 150 characters.")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Author ID must be positive.")]
        public int? AuthorId { get; set; }

        public bool IsAvailable { get; set; } = true;

    }
}
