﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace inspectores_api.dbContext;

public partial class InspectoresContext : DbContext
{
    public InspectoresContext()
    {
    }

    public InspectoresContext(DbContextOptions<InspectoresContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Funcione> Funciones { get; set; }

    public virtual DbSet<Inspectore> Inspectores { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("pgcrypto");

        modelBuilder.Entity<Funcione>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("funciones_pkey");

            entity.ToTable("funciones");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.NameFuncion)
                .HasMaxLength(255)
                .HasColumnName("name_funcion");
        });

        modelBuilder.Entity<Inspectore>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("inspectores_pkey");

            entity.ToTable("inspectores");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Activo).HasColumnName("activo");
            entity.Property(e => e.Documento)
                .HasMaxLength(255)
                .HasColumnName("documento");
            entity.Property(e => e.FuncionId).HasColumnName("funcion_id");
            entity.Property(e => e.HashLagajo).HasColumnName("hash_lagajo");
            entity.Property(e => e.NombreCompleto)
                .HasMaxLength(255)
                .HasColumnName("nombre_completo");
            entity.Property(e => e.NumeroAfiliado)
                .HasMaxLength(255)
                .HasColumnName("numero_afiliado");
            entity.Property(e => e.QrBase64).HasColumnName("qr_base_64");
            entity.Property(e => e.UrlImagen).HasColumnName("url_imagen");

            entity.HasOne(d => d.Funcion).WithMany(p => p.Inspectores)
                .HasForeignKey(d => d.FuncionId)
                .HasConstraintName("inspectores_funcion_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
