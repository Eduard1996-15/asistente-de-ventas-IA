using System;
using System.Collections.Generic;

namespace asistenteventas.Models
{
    public partial class Stock
    {
#pragma warning disable CS8618 // El elemento propiedad "Imagen" que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declarar el elemento propiedad como que admite un valor NULL.
        public Stock()
#pragma warning restore CS8618 // El elemento propiedad "Imagen" que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declarar el elemento propiedad como que admite un valor NULL.
        {
            Facloc1s = new HashSet<Facloc1>();
        }

        public string CodArt { get; set; } = null!;
        public string? DesArt { get; set; }
        public string? DesDetArt { get; set; }
        public string? CodMod { get; set; }
        public string? CodColPrv { get; set; }
        public string? CodTalPrv { get; set; }
        public string? CodArtAlt { get; set; }
        public int SalWebArt { get; set; }

        public virtual Modelo? CodModNavigation { get; set; }
        public virtual ICollection<Facloc1> Facloc1s { get; set; }

        public override bool Equals(object? obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
