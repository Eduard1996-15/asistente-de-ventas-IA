using System;
using System.Collections.Generic;

namespace asistenteventas.Models
{
    public partial class Facloc
    {
        public Facloc()
        {
            Facloc1s = new HashSet<Facloc1>();
        }

        public short CodLoc { get; set; }
        public short CodTipDocF { get; set; }
        public string SerFacLoc { get; set; } = null!;
        public int NroFacLoc { get; set; }
        public DateTime? FecFacLoc { get; set; }
        public decimal? CodCli { get; set; }
        public short? CodMon { get; set; }
        public decimal? CotFacLoc { get; set; }
        public decimal? CotComFacL { get; set; }
        public decimal? CotVenFacL { get; set; }
        public decimal? TotFacLoc { get; set; }

        public virtual Client? CodCliNavigation { get; set; }
        public virtual ICollection<Facloc1> Facloc1s { get; set; }
    }
}
