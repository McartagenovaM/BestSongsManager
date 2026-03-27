using SongManager.Models;

namespace SongManager.Services;

public interface ISongService
{
    Task<List<Song>> GetAllSongsAsync();
    Task<Song?> GetSongByIdAsync(int id);
    Task<Song> CreateSongAsync(Song song);
    Task<Song> UpdateSongAsync(Song song);
    Task DeleteSongAsync(int id);
    Task<Song?> ToggleFavoriteAsync(int id);
    Task<List<Song>> GetFavoritesAsync();
    Task<List<Song>> SearchSongsAsync(string query);
}
