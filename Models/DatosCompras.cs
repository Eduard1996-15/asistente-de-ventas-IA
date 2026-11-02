namespace asistenteventas.Models
{
    public class DatosCompras
    {
        public int Id { get; set; }
        public decimal usuario { get; set; }   
        public string producto { get; set; }
        public int calificacion { get; set; }

        public string comentario { get; set; }
        public DatosCompras(decimal usuario, string producto, int calificacion, string comentario)
        {
            this.usuario = usuario;
            this.producto = producto;
            this.calificacion = calificacion;
            this.comentario = comentario;
            this.comentario= comentario;
        }

        public DatosCompras()
        {
        }

        public override bool Equals(object? obj)
        {
            return obj is DatosCompras compras &&
                   usuario == compras.usuario;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(usuario);
        }
    }
}
