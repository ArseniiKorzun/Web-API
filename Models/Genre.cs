namespace MusicApi.Models;

public class Genre
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<Album> Albums { get; set; } = [];
}
