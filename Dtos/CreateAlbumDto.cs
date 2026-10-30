using System.ComponentModel.DataAnnotations;

namespace MusicApi.Dtos;

public class CreateAlbumDto
{
    [Required]
    [StringLength(150, MinimumLength = 1)]
    public string Title { get; set; } = string.Empty;

    public int Year { get; set; }

    public decimal Price { get; set; }

    public int ArtistId { get; set; }

    public int GenreId { get; set; }
}
