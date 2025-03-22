using System;
using System.Collections.Generic;

namespace asistenteventas.Models
{
    public partial class Facloc1
    {
        public short CodLoc { get; set; }
        public short CodTipDocF { get; set; }
        public string SerFacLoc { get; set; } = null!;
        public int NroFacLoc { get; set; }
        public string CodArt { get; set; } = null!;
        public short? CanFacLoc { get; set; }
        public decimal? PreUniFacL { get; set; }
        public decimal? PorDtoFacL { get; set; }

        public virtual Stock CodArtNavigation { get; set; } = null!;
        public virtual Facloc Facloc { get; set; } = null!;
    }
}
