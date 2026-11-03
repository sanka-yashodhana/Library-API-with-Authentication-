using System.ComponentModel.DataAnnotations;

namespace LibraryApi.Dtos
{
    public class CreateAuthorDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
    }
}
