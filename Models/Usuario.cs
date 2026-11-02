using System;
using System.Collections.Generic;

using System.ComponentModel.DataAnnotations;
namespace asistenteventas.Models
{
    public partial class Usuario
    {

        [Key]
        public int id { get; set; }
        public string nombre { get; set; } = null!;
        public string passsword { get; set; } = null!;

        public int rol { get; set; } 



        public Usuario()
        {
        }

        public override bool Equals(object? obj)
        {
            return obj is Usuario usuario &&
                   id == usuario.id &&
                   nombre == usuario.nombre &&
                   passsword == usuario.passsword &&
                   rol == usuario.rol;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(id, nombre, passsword, rol);
        }
    }
}
