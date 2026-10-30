using MusicApi.Dtos;

namespace MusicApi.Services;

public interface IAlbumService
{
    Task<List<AlbumDto>> GetAllAsync();
    Task<AlbumDto?> GetByIdAsync(int id);
    Task<List<AlbumDto>> SearchAsync(AlbumFilter filter);
    Task<AlbumDto> CreateAsync(CreateAlbumDto dto);
    Task<bool> DeleteAsync(int id);
}
