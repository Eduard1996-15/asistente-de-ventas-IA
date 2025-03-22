using System;
using System.Collections.Generic;

namespace asistenteventas.Models
{
    public partial class Modelo
    {
        public Modelo()
        {
            Stocks = new HashSet<Stock>();
        }

        public string CodMod { get; set; } = null!;
        public string? DesMod { get; set; }
        public string? CodTem { get; set; }
        public short? CodFam { get; set; }
        public short? CodFam2 { get; set; }
        public short? CodPrv { get; set; }
        public short? OrdCreMod { get; set; }
        public short? CodMonVen { get; set; }
        public string? ModPreArt { get; set; }
        public string? SexMod { get; set; }
        public string? DesEshop { get; set; }
        public short? NueMod { get; set; }
        public short? SalMod { get; set; }
        public short OutMod { get; set; }
        public string? ComMod { get; set; }
        public string? LarMod { get; set; }
        public string? ManMod { get; set; }

        public virtual ICollection<Stock> Stocks { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is Modelo modelo &&
                   CodMod == modelo.CodMod &&
                   DesMod == modelo.DesMod &&
                   CodTem == modelo.CodTem &&
                   CodFam == modelo.CodFam &&
                   CodFam2 == modelo.CodFam2 &&
                   CodPrv == modelo.CodPrv &&
                   OrdCreMod == modelo.OrdCreMod &&
                   CodMonVen == modelo.CodMonVen &&
                   ModPreArt == modelo.ModPreArt &&
                   SexMod == modelo.SexMod &&
                   DesEshop == modelo.DesEshop &&
                   NueMod == modelo.NueMod &&
                   SalMod == modelo.SalMod &&
                   OutMod == modelo.OutMod &&
                   ComMod == modelo.ComMod &&
                   LarMod == modelo.LarMod &&
                   ManMod == modelo.ManMod &&
                   EqualityComparer<ICollection<Stock>>.Default.Equals(Stocks, modelo.Stocks);
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(CodMod);
            hash.Add(DesMod);
            hash.Add(CodTem);
            hash.Add(CodFam);
            hash.Add(CodFam2);
            hash.Add(CodPrv);
            hash.Add(OrdCreMod);
            hash.Add(CodMonVen);
            hash.Add(ModPreArt);
            hash.Add(SexMod);
            hash.Add(DesEshop);
            hash.Add(NueMod);
            hash.Add(SalMod);
            hash.Add(OutMod);
            hash.Add(ComMod);
            hash.Add(LarMod);
            hash.Add(ManMod);
            hash.Add(Stocks);
            return hash.ToHashCode();
        }
    }
}
