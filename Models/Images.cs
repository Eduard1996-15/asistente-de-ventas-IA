using System.ComponentModel.DataAnnotations;

namespace asistenteventas.Models
{
    public partial class Images
    {
		[Key]
		public string CodArt { get; set; }
        public byte[] Imagen { get; set; }
        public string CodMod { get; set; }
    }
}