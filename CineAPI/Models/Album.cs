namespace CineAPI.Models
{
	public class Album
	{
		public int Id { get; set; }

		public string Titulo { get; set; } = string.Empty;

		public string Artista { get; set; } = string.Empty;

		public int AnioLanzamiento { get; set; }

		public string Disquera { get; set; } = string.Empty;

		public int GeneroId { get; set; }

		public Genero? Genero { get; set; }
		public string Descripcion { get; set; } = string.Empty;
		public decimal Precio { get; set; }
		public string Imagen { get; set; } = string.Empty;
	}
}