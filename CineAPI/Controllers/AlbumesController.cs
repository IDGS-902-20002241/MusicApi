using CineAPI.Data;
using CineAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CineAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AlbumesController : ControllerBase
	{
		private readonly AppDbContext _context;

		public AlbumesController(AppDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Album>>> GetAlbumes()
		{
			return await _context.Albumes
				.Include(a => a.Genero)
				.ToListAsync();
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Album>> GetAlbum(int id)
		{
			var album = await _context.Albumes
				.Include(a => a.Genero)
				.FirstOrDefaultAsync(a => a.Id == id);

			if (album == null)
			{
				return NotFound();
			}

			return album;
		}

		[HttpPost]
		public async Task<ActionResult<Album>> PostAlbum(Album album)
		{
			var generoExiste = await _context.Generos.AnyAsync(g => g.Id == album.GeneroId);

			if (!generoExiste)
			{
				return BadRequest("El género no existe.");
			}

			_context.Albumes.Add(album);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(GetAlbum), new { id = album.Id }, album);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> PutAlbum(int id, Album album)
		{
			if (id != album.Id)
			{
				return BadRequest();
			}

			var generoExiste = await _context.Generos.AnyAsync(g => g.Id == album.GeneroId);

			if (!generoExiste)
			{
				return BadRequest("El género no existe.");
			}

			_context.Entry(album).State = EntityState.Modified;
			await _context.SaveChangesAsync();

			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteAlbum(int id)
		{
			var album = await _context.Albumes.FindAsync(id);

			if (album == null)
			{
				return NotFound();
			}

			_context.Albumes.Remove(album);
			await _context.SaveChangesAsync();

			return NoContent();
		}
	}
}