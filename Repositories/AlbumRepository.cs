using Microsoft.EntityFrameworkCore;
using MusicApi.Data;
using MusicApi.Dtos;
using MusicApi.Models;

namespace MusicApi.Repositories;

public class AlbumRepository(AppDbContext db) : IAlbumRepository
{
    private IQueryable<Album> AlbumsWithDetails => db.Albums
        .AsNoTracking()
        .Include(a => a.Artist)
        .Include(a => a.Genre);

    public Task<List<Album>> GetAllAsync() =>
        AlbumsWithDetails.OrderBy(a => a.Id).ToListAsync();

    public Task<Album?> GetByIdAsync(int id) =>
        AlbumsWithDetails.FirstOrDefaultAsync(a => a.Id == id);

    public Task<List<Album>> SearchAsync(AlbumFilter filter)
    {
        var query = AlbumsWithDetails;

        if (!string.IsNullOrWhiteSpace(filter.Title))
            query = query.Where(a => EF.Functions.Like(a.Title, $"%{filter.Title.Trim()}%"));

        if (!string.IsNullOrWhiteSpace(filter.Artist))
            query = query.Where(a => EF.Functions.Like(a.Artist!.Name, $"%{filter.Artist.Trim()}%"));

        if (!string.IsNullOrWhiteSpace(filter.Genre))
            query = query.Where(a => EF.Functions.Like(a.Genre!.Name, filter.Genre.Trim()));

        if (filter.MinYear.HasValue)
            query = query.Where(a => a.Year >= filter.MinYear.Value);

        if (filter.MaxYear.HasValue)
            query = query.Where(a => a.Year <= filter.MaxYear.Value);

        if (filter.MaxPrice.HasValue)
            query = query.Where(a => a.Price <= filter.MaxPrice.Value);

        return query.OrderBy(a => a.Year).ThenBy(a => a.Title).ToListAsync();
    }

    public Task<bool> ArtistExistsAsync(int artistId) =>
        db.Artists.AnyAsync(a => a.Id == artistId);

    public Task<bool> GenreExistsAsync(int genreId) =>
        db.Genres.AnyAsync(g => g.Id == genreId);

    public Task<bool> TitleExistsForArtistAsync(int artistId, string title) =>
        db.Albums.AnyAsync(a => a.ArtistId == artistId && a.Title.ToLower() == title.ToLower());

    public async Task AddAsync(Album album)
    {
        db.Albums.Add(album);
        await db.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id) =>
        await db.Albums.Where(a => a.Id == id).ExecuteDeleteAsync() > 0;
}
