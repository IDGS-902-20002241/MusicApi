using System.Text.Json.Serialization;

namespace CineAPI.Models
{
	public class Genero
	{
		public int Id { get; set; }

		public string Nombre { get; set; } = string.Empty;

		[JsonIgnore]
		public List<Album> Albumes { get; set; } = new();
	}
}