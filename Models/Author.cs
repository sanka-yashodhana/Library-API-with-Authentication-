using System.ComponentModel.DataAnnotations;

namespace LibraryApi.Models
{
    public class Author
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        public List<Book> Books { get; set; } = new();
    }
}