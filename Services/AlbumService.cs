using MusicApi.Dtos;
using MusicApi.Models;
using MusicApi.Repositories;

namespace MusicApi.Services;

public class AlbumService(IAlbumRepository repository) : IAlbumService
{
    private const int FirstRecordYear = 1900;
    private const decimal MaxPrice = 1000m;

    public async Task<List<AlbumDto>> GetAllAsync() =>
        (await repository.GetAllAsync()).Select(AlbumDto.From).ToList();

    public async Task<AlbumDto?> GetByIdAsync(int id)
    {
        var album = await repository.GetByIdAsync(id);
        return album is null ? null : AlbumDto.From(album);
    }

    public async Task<List<AlbumDto>> SearchAsync(AlbumFilter filter)
    {
        if (filter.MinYear > filter.MaxYear)
            throw new AlbumValidationException("minYear не може бути більшим за maxYear.");

        return (await repository.SearchAsync(filter)).Select(AlbumDto.From).ToList();
    }

    public async Task<AlbumDto> CreateAsync(CreateAlbumDto dto)
    {
        var title = dto.Title.Trim();
        var currentYear = DateTime.UtcNow.Year;

        if (string.IsNullOrEmpty(title))
            throw new AlbumValidationException("Назва альбому не може бути порожньою.");

        if (dto.Year < FirstRecordYear || dto.Year > currentYear)
            throw new AlbumValidationException($"Рік випуску має бути в межах {FirstRecordYear}–{currentYear}.");

        if (dto.Price <= 0 || dto.Price > MaxPrice)
            throw new AlbumValidationException($"Ціна має бути більшою за 0 і не більшою за {MaxPrice}.");

        if (!await repository.ArtistExistsAsync(dto.ArtistId))
            throw new AlbumValidationException($"Виконавця з id {dto.ArtistId} не існує.");

        if (!await repository.GenreExistsAsync(dto.GenreId))
            throw new AlbumValidationException($"Жанру з id {dto.GenreId} не існує.");

        if (await repository.TitleExistsForArtistAsync(dto.ArtistId, title))
            throw new AlbumValidationException($"У цього виконавця вже є альбом «{title}».");

        var album = new Album
        {
            Title = title,
            Year = dto.Year,
            Price = Math.Round(dto.Price, 2),
            ArtistId = dto.ArtistId,
            GenreId = dto.GenreId
        };

        await repository.AddAsync(album);
        return (await GetByIdAsync(album.Id))!;
    }

    public Task<bool> DeleteAsync(int id) => repository.DeleteAsync(id);
}
