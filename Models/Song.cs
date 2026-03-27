namespace SongManager.Models;

public class Song
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Artist { get; set; } = string.Empty;
    public string Album { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
    public int Year { get; set; }
    public bool IsFavorite { get; set; }
    public string Notes { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string PlaybackUrl { get; set; } = string.Empty;
    public DateTime DateAdded { get; set; }
    public string Duration { get; set; } = string.Empty;
}
