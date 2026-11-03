
namespace LibraryApi.Dtos
{
    public class BookResponseDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Author { get; set; } = string.Empty;

        public bool IsAvailable { get; set; }
    }
}
