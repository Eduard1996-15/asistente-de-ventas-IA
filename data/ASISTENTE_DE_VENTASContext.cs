using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using asistenteventas.Models;

namespace asistenteventas.Data
{
    public partial class ASISTENTE_DE_VENTASContext : DbContext
    {
		private DbSet<Images> images;

		public ASISTENTE_DE_VENTASContext()
        {
        }

        public ASISTENTE_DE_VENTASContext(DbContextOptions<ASISTENTE_DE_VENTASContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Usuario> Usuarios { get; set; } = null!;
        public virtual DbSet<Client> Clients { get; set; } = null!;
        public virtual DbSet<Facloc> Faclocs { get; set; } = null!;
        public virtual DbSet<Facloc1> Facloc1s { get; set; } = null!;
        public virtual DbSet<Modelo> Modelos { get; set; } = null!;
        public virtual DbSet<Stock> Stocks { get; set; } = null!;
        public virtual DbSet<CodigoColor> CodigosColor { get; set; } = null!;
		public virtual DbSet<Images> Images { get; set; } = null!;
		public virtual DbSet<DatosCompras> DatosCompras { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=ConnectionStrings:conexion");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            modelBuilder.Entity<DatosCompras>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_datosCompras");
                entity.ToTable("DatosCompras");
                entity.ToTable(nameof(DatosCompras));
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.usuario).HasColumnType("decimal(12, 0)").HasColumnName("usuario");
                entity.Property(e => e.producto).HasMaxLength(30)
                    .IsUnicode(false).HasColumnName("producto");
                entity.Property(e => e.calificacion).HasColumnName("calificacion");
                entity.Property(e => e.comentario).HasColumnName("comentario");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
               
                  entity.HasKey(e => e.id);
                entity.ToTable("Usuario");
              

                entity.Property(e => e.id)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("Id");
                entity.Property(e => e.passsword)
                   .HasMaxLength(10)
                   .IsUnicode(false)
                   .HasColumnName("Contrasenia");
                entity.Property(e => e.nombre)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("Nombre");
                entity.Property(e => e.rol)
                    .HasColumnName("RolId");


            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasKey(e => e.CodCli)
                    .HasName("PK__CLIENT__98E6AA36DD9CAE0B");

                entity.ToTable("CLIENT");

                entity.Property(e => e.CodCli).HasColumnType("decimal(12, 0)");

                entity.Property(e => e.CarTelCli)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.CelCli)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.CiuCli)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.CodPais)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.DirCli)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.FecActCli).HasColumnType("datetime");

                entity.Property(e => e.FecAltCli).HasColumnType("datetime");

                entity.Property(e => e.FecNacCli).HasColumnType("datetime");

                entity.Property(e => e.FlgCliLoc)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.HorActCli)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.MailCli)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.NroDocCli)
                    .HasMaxLength(14)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.ObsCli)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.PriApeCli)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.PriNomCli)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.SegApeCli)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.SegNomCli)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.SexCli)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.TelCli)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.UltCmpCli).HasColumnType("datetime");
            });

            modelBuilder.Entity<Facloc>(entity =>
            {
                entity.HasKey(e => new { e.CodLoc, e.CodTipDocF, e.SerFacLoc, e.NroFacLoc })
                    .HasName("PK__FACLOC__A0F4D262FE07D1F3");

                entity.ToTable("FACLOC");

                entity.Property(e => e.SerFacLoc)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.CodCli).HasColumnType("decimal(12, 0)");

                entity.Property(e => e.CotComFacL).HasColumnType("smallmoney");

                entity.Property(e => e.CotFacLoc).HasColumnType("smallmoney");

                entity.Property(e => e.CotVenFacL).HasColumnType("smallmoney");

                entity.Property(e => e.FecFacLoc).HasColumnType("datetime");

                entity.Property(e => e.TotFacLoc).HasColumnType("money");

                entity.HasOne(d => d.CodCliNavigation)
                    .WithMany(p => p.Faclocs)
                    .HasForeignKey(d => d.CodCli)
                    .HasConstraintName("FK__FACLOC__CodCli__4316F928");
            });

            modelBuilder.Entity<Facloc1>(entity =>
            {
                entity.HasKey(e => new { e.CodLoc, e.CodTipDocF, e.SerFacLoc, e.NroFacLoc, e.CodArt })
                    .HasName("PK__FACLOC1__6DF94A0431BC77A5");

                entity.ToTable("FACLOC1");

                entity.Property(e => e.SerFacLoc)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.CodArt)
                    .HasMaxLength(19)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.PorDtoFacL).HasColumnType("smallmoney");

                entity.Property(e => e.PreUniFacL).HasColumnType("money");

                entity.HasOne(d => d.CodArtNavigation)
                    .WithMany(p => p.Facloc1s)
                    .HasForeignKey(d => d.CodArt)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__FACLOC1__CodArt__45F365D3");

                entity.HasOne(d => d.Facloc)
                    .WithMany(p => p.Facloc1s)
                    .HasForeignKey(d => new { d.CodLoc, d.CodTipDocF, d.SerFacLoc, d.NroFacLoc })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FACLOC1");
            });

            modelBuilder.Entity<Modelo>(entity =>
            {
                entity.HasKey(e => e.CodMod)
                    .HasName("PK__MODELO__837D533A143397B7");

                entity.ToTable("MODELO");

                entity.Property(e => e.CodMod)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.CodTem)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.ComMod)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.DesEshop)
                    .HasMaxLength(700)
                    .IsUnicode(false)
                    .HasColumnName("DesEShop");

                entity.Property(e => e.DesMod)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.LarMod)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.ManMod)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.ModPreArt)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.SexMod)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Stock>(entity =>
            {
                entity.HasKey(e => e.CodArt)
                    .HasName("PK__STOCK__9866CD0C205A0821");

                entity.ToTable("STOCK");

                entity.Property(e => e.CodArt)
                    .HasMaxLength(19)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.CodArtAlt)
                    .HasMaxLength(18)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.CodColPrv)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.CodMod)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.CodTalPrv)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.DesArt)
                    .HasMaxLength(35)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.DesDetArt)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.CodModNavigation)
                    .WithMany(p => p.Stocks)
                    .HasForeignKey(d => d.CodMod)
                    .HasConstraintName("FK__STOCK__CodMod__403A8C7D");
            });

            


            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
