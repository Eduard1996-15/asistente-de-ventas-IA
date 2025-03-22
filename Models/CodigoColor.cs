using System.ComponentModel.DataAnnotations;

namespace asistenteventas.Models
{
    public partial class CodigoColor
    {
        [Key]
        public string? CodColPrv { get; set; }
        public string? CodCol { get; set; }
        public string? Nombre { get; set; }

    }
}
