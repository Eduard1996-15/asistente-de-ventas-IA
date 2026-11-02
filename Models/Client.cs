using System;
using System.Collections.Generic;

namespace asistenteventas.Models
{
    public partial class Client
    {
        public Client()
        {
            Faclocs = new HashSet<Facloc>();
        }

        public decimal CodCli { get; set; }
        public string? DirCli { get; set; }
        public string? NroDocCli { get; set; }
        public string TelCli { get; set; } = " Sin teléfono "; // valor predeterminado
        public string MailCli { get; set; } = " Sin mail ";
        public DateTime? FecNacCli { get; set; }
        public string? ObsCli { get; set; }
        public DateTime? FecAltCli { get; set; }
        public int? LugAltCli { get; set; }
        public DateTime? FecActCli { get; set; }
        public string? HorActCli { get; set; }
        public int? LugActCli { get; set; }
        public string? FlgCliLoc { get; set; }
        public short? CodDpt { get; set; }
        public short? CodZon { get; set; }
        public string? PriNomCli { get; set; }
        public string? SegNomCli { get; set; }
        public string? PriApeCli { get; set; }
        public string? SegApeCli { get; set; }
        public short? NroNomCli { get; set; }
        public string? SexCli { get; set; }
        public string? CelCli { get; set; }
        public DateTime? UltCmpCli { get; set; }
        public string? CodPais { get; set; }
        public short? TipDocCli { get; set; }
        public string? CiuCli { get; set; }
        public short? AltWebCli { get; set; }
        public string? CarTelCli { get; set; }

        public virtual ICollection<Facloc> Faclocs { get; set; }
    }
}
