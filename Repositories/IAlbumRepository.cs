using MusicApi.Dtos;
using MusicApi.Models;

namespace MusicApi.Repositories;

public interface IAlbumRepository
{
    Task<List<Album>> GetAllAsync();
    Task<Album?> GetByIdAsync(int id);
    Task<List<Album>> SearchAsync(AlbumFilter filter);
    Task<bool> ArtistExistsAsync(int artistId);
    Task<bool> GenreExistsAsync(int genreId);
    Task<bool> TitleExistsForArtistAsync(int artistId, string title);
    Task AddAsync(Album album);
    Task<bool> DeleteAsync(int id);
}
