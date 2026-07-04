using CineAPI.Data;
using CineAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CineAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class GenerosController : ControllerBase
	{
		private readonly AppDbContext _context;

		public GenerosController(AppDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Genero>>> GetGeneros()
		{
			return await _context.Generos.ToListAsync();
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Genero>> GetGenero(int id)
		{
			var genero = await _context.Generos.FindAsync(id);

			if (genero == null)
			{
				return NotFound();
			}

			return genero;
		}

		[HttpPost]
		public async Task<ActionResult<Genero>> PostGenero(Genero genero)
		{
			_context.Generos.Add(genero);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(GetGenero), new { id = genero.Id }, genero);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> PutGenero(int id, Genero genero)
		{
			if (id != genero.Id)
			{
				return BadRequest();
			}

			_context.Entry(genero).State = EntityState.Modified;
			await _context.SaveChangesAsync();

			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteGenero(int id)
		{
			var genero = await _context.Generos.FindAsync(id);

			if (genero == null)
			{
				return NotFound();
			}

			_context.Generos.Remove(genero);
			await _context.SaveChangesAsync();

			return NoContent();
		}
	}
}