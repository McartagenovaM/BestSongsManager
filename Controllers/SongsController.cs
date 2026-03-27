using Microsoft.AspNetCore.Mvc;
using SongManager.Models;
using SongManager.Services;

namespace SongManager.Controllers;

public class SongsController : Controller
{
    private readonly ISongService _songService;

    public SongsController(ISongService songService)
    {
        _songService = songService;
    }

    public async Task<IActionResult> Index()
    {
        var songs = await _songService.GetAllSongsAsync();
        return View(songs);
    }

    public async Task<IActionResult> Details(int id)
    {
        var song = await _songService.GetSongByIdAsync(id);
        if (song == null)
        {
            return NotFound();
        }
        return View(song);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Song song)
    {
        if (ModelState.IsValid)
        {
            await _songService.CreateSongAsync(song);
            return RedirectToAction(nameof(Index));
        }
        return View(song);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var song = await _songService.GetSongByIdAsync(id);
        if (song == null)
        {
            return NotFound();
        }
        return View(song);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Song song)
    {
        if (id != song.Id)
        {
            return BadRequest();
        }

        if (ModelState.IsValid)
        {
            await _songService.UpdateSongAsync(song);
            return RedirectToAction(nameof(Details), new { id = song.Id });
        }
        return View(song);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var song = await _songService.GetSongByIdAsync(id);
        if (song == null)
        {
            return NotFound();
        }

        await _songService.DeleteSongAsync(id);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Favorites()
    {
        var songs = await _songService.GetFavoritesAsync();
        return View(songs);
    }

    [HttpPost]
    public async Task<IActionResult> ToggleFavorite(int id)
    {
        var song = await _songService.ToggleFavoriteAsync(id);
        if (song == null)
        {
            return NotFound();
        }
        return RedirectToAction(nameof(Details), new { id = id });
    }
}
