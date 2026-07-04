using Microsoft.EntityFrameworkCore;
using CineAPI.Data;
using CineAPI.Models;
var builder = WebApplication.CreateBuilder(args);

// 1. Configurar SQL Server
builder.Services.AddSqlServer<AppDbContext>(
	builder.Configuration.GetConnectionString("cadenaSQL")
);

// 2. Configurar CORS para Angular
builder.Services.AddCors(options =>
{
	options.AddPolicy("NuevaPolitica", app =>
	{
		app.AllowAnyOrigin()
		   .AllowAnyHeader()
		   .AllowAnyMethod();
	});
});

// Evitar ciclos al serializar JSON
builder.Services.ConfigureHttpJsonOptions(options =>
{
	options.SerializerOptions.ReferenceHandler =
		System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});

var app = builder.Build();

app.UseCors("NuevaPolitica");

// Crear Base de Datos
app.MapGet("/crearbd", async (AppDbContext db) =>
{
	await db.Database.MigrateAsync();
	return Results.Ok("Base de datos de música creada con éxito");
});

// GET: Obtener todos los géneros
app.MapGet("/api/generos", async (AppDbContext db) =>
{
	return await db.Generos.ToListAsync();
});

// POST: Agregar género
app.MapPost("/api/generos", async (AppDbContext db, Genero genero) =>
{
	db.Generos.Add(genero);
	await db.SaveChangesAsync();

	return Results.Ok(genero);
});

// GET: Obtener todos los álbumes con su género
app.MapGet("/api/albumes", async (AppDbContext db) =>
{
	return await db.Albumes
		.Include(a => a.Genero)
		.ToListAsync();
});

// GET: Buscar álbumes por título o artista
app.MapGet("/api/albumes/buscar/{texto}", async (AppDbContext db, string texto) =>
{
	return await db.Albumes
		.Include(a => a.Genero)
		.Where(a =>
			a.Titulo.Contains(texto) ||
			a.Artista.Contains(texto)
		)
		.ToListAsync();
});

// GET: Obtener un álbum por id
app.MapGet("/api/albumes/{id}", async (AppDbContext db, int id) =>
{
	var album = await db.Albumes
		.Include(a => a.Genero)
		.FirstOrDefaultAsync(a => a.Id == id);

	if (album == null)
	{
		return Results.NotFound("Álbum no encontrado");
	}

	return Results.Ok(album);
});

// POST: Agregar un nuevo álbum
app.MapPost("/api/albumes", async (AppDbContext db, Album album) =>
{
	var generoExiste = await db.Generos.AnyAsync(g => g.Id == album.GeneroId);

	if (!generoExiste)
	{
		return Results.BadRequest("El género seleccionado no existe");
	}

	db.Albumes.Add(album);
	await db.SaveChangesAsync();

	return Results.Ok(album);
});

// PUT: Modificar un álbum
app.MapPut("/api/albumes/{id}", async (AppDbContext db, int id, Album datosNuevos) =>
{
	var album = await db.Albumes.FindAsync(id);

	if (album == null)
	{
		return Results.NotFound("Álbum no encontrado");
	}

	var generoExiste = await db.Generos.AnyAsync(g => g.Id == datosNuevos.GeneroId);

	if (!generoExiste)
	{
		return Results.BadRequest("El género seleccionado no existe");
	}

	album.Titulo = datosNuevos.Titulo;
	album.Artista = datosNuevos.Artista;
	album.AnioLanzamiento = datosNuevos.AnioLanzamiento;
	album.Disquera = datosNuevos.Disquera;
	album.Descripcion = datosNuevos.Descripcion;
	album.Precio = datosNuevos.Precio;
	album.Imagen = datosNuevos.Imagen;
	album.GeneroId = datosNuevos.GeneroId;

	await db.SaveChangesAsync();

	return Results.Ok(album);
});

// DELETE: Eliminar un álbum
app.MapDelete("/api/albumes/{id}", async (AppDbContext db, int id) =>
{
	var album = await db.Albumes.FindAsync(id);

	if (album == null)
	{
		return Results.NotFound("Álbum no encontrado");
	}

	db.Albumes.Remove(album);
	await db.SaveChangesAsync();

	return Results.Ok(new { mensaje = "Álbum eliminado correctamente" });
});

app.Run();