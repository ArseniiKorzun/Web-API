namespace MusicApi.Dtos;

public class AlbumFilter
{
    public string? Title { get; set; }
    public string? Artist { get; set; }
    public string? Genre { get; set; }
    public int? MinYear { get; set; }
    public int? MaxYear { get; set; }
    public decimal? MaxPrice { get; set; }
}
