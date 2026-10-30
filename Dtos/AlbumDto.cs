using MusicApi.Models;

namespace MusicApi.Dtos;

public record AlbumDto(int Id, string Title, int Year, decimal Price, int ArtistId, string Artist, int GenreId, string Genre)
{
    public static AlbumDto From(Album album) => new(
        album.Id,
        album.Title,
        album.Year,
        album.Price,
        album.ArtistId,
        album.Artist?.Name ?? string.Empty,
        album.GenreId,
        album.Genre?.Name ?? string.Empty);
}
