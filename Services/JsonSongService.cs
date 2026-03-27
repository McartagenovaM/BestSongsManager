using System.Text.Json;
using SongManager.Models;

namespace SongManager.Services;

public class JsonSongService : ISongService
{
    private readonly string _jsonFilePath;
    private readonly object _fileLock = new();
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

    public JsonSongService(IWebHostEnvironment environment)
    {
        var dataDirectory = Path.Combine(environment.ContentRootPath, "Data");
        _jsonFilePath = Path.Combine(dataDirectory, "songs.json");

        if (!Directory.Exists(dataDirectory))
        {
            Directory.CreateDirectory(dataDirectory);
        }
    }

    public async Task<List<Song>> GetAllSongsAsync()
    {
        return await LoadSongsAsync();
    }

    public async Task<Song?> GetSongByIdAsync(int id)
    {
        var songs = await LoadSongsAsync();
        return songs.FirstOrDefault(s => s.Id == id);
    }

    public async Task<Song> CreateSongAsync(Song song)
    {
        var songs = await LoadSongsAsync();
        song.Id = songs.Count > 0 ? songs.Max(s => s.Id) + 1 : 1;
        NormalizeSong(song);
        songs.Add(song);
        await SaveSongsAsync(songs);
        return song;
    }

    public async Task<Song> UpdateSongAsync(Song song)
    {
        var songs = await LoadSongsAsync();
        var index = songs.FindIndex(s => s.Id == song.Id);
        if (index >= 0)
        {
            MergeMissingValues(song, songs[index]);
            NormalizeSong(song);
            songs[index] = song;
            await SaveSongsAsync(songs);
        }
        return song;
    }

    public async Task DeleteSongAsync(int id)
    {
        var songs = await LoadSongsAsync();
        songs.RemoveAll(s => s.Id == id);
        await SaveSongsAsync(songs);
    }

    public async Task<Song?> ToggleFavoriteAsync(int id)
    {
        var songs = await LoadSongsAsync();
        var song = songs.FirstOrDefault(s => s.Id == id);
        if (song != null)
        {
            song.IsFavorite = !song.IsFavorite;
            await SaveSongsAsync(songs);
        }
        return song;
    }

    public async Task<List<Song>> GetFavoritesAsync()
    {
        var songs = await LoadSongsAsync();
        return songs.Where(s => s.IsFavorite).ToList();
    }

    public async Task<List<Song>> SearchSongsAsync(string query)
    {
        var songs = await LoadSongsAsync();
        var lowerQuery = query.ToLower();
        return songs.Where(s =>
            s.Title.ToLower().Contains(lowerQuery) ||
            s.Artist.ToLower().Contains(lowerQuery) ||
            s.Album.ToLower().Contains(lowerQuery) ||
            s.Genre.ToLower().Contains(lowerQuery)
        ).ToList();
    }

    private Task<List<Song>> LoadSongsAsync()
    {
        return Task.Run(() =>
        {
            lock (_fileLock)
            {
                try
                {
                    if (!File.Exists(_jsonFilePath))
                    {
                        return new List<Song>();
                    }

                    var json = File.ReadAllText(_jsonFilePath);
                    if (string.IsNullOrWhiteSpace(json))
                    {
                        return new List<Song>();
                    }

                    var songs = JsonSerializer.Deserialize<List<Song>>(json, SerializerOptions);
                    if (songs == null)
                    {
                        return new List<Song>();
                    }

                    foreach (var song in songs)
                    {
                        NormalizeSong(song);
                    }

                    return songs;
                }
                catch (JsonException)
                {
                    return new List<Song>();
                }
                catch (IOException)
                {
                    return new List<Song>();
                }
            }
        });
    }

    private Task SaveSongsAsync(List<Song> songs)
    {
        return Task.Run(() =>
        {
            lock (_fileLock)
            {
                try
                {
                    var json = JsonSerializer.Serialize(songs, SerializerOptions);
                    File.WriteAllText(_jsonFilePath, json);
                }
                catch (IOException)
                {
                    // Handle file write errors gracefully
                }
            }
        });
    }

    private static void NormalizeSong(Song song)
    {
        if (song.DateAdded == default)
        {
            song.DateAdded = DateTime.UtcNow;
        }

        if (string.IsNullOrWhiteSpace(song.Duration))
        {
            song.Duration = "0:00";
        }

        song.ImageUrl ??= string.Empty;
        song.PlaybackUrl ??= string.Empty;
    }

    private static void MergeMissingValues(Song incomingSong, Song existingSong)
    {
        if (incomingSong.DateAdded == default)
        {
            incomingSong.DateAdded = existingSong.DateAdded;
        }

        if (string.IsNullOrWhiteSpace(incomingSong.Duration))
        {
            incomingSong.Duration = existingSong.Duration;
        }

        if (string.IsNullOrWhiteSpace(incomingSong.ImageUrl))
        {
            incomingSong.ImageUrl = existingSong.ImageUrl;
        }

        if (string.IsNullOrWhiteSpace(incomingSong.PlaybackUrl))
        {
            incomingSong.PlaybackUrl = existingSong.PlaybackUrl;
        }
    }
}
