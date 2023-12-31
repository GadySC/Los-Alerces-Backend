﻿using LosAlerces_DBManagement.Entities;
using Microsoft.EntityFrameworkCore;

namespace LosAlerces_DBManagement.Context
{
    public class LosAlercesDbContext : DbContext
    {
        public LosAlercesDbContext(DbContextOptions<LosAlercesDbContext> options)
            : base(options)
        {
        }

        // DbSets para tus entidades
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Productos> Productos { get; set; }
        public DbSet<Personal> Personal { get; set; }
        public DbSet<Cotizacion> Cotizacion { get; set; }
        public DbSet<ProductoCotizacion> ProductoCotizacion { get; set; }
        public DbSet<PersonalCotizacion> PersonalCotizacion { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configuración de la relación entre Cliente y Contactos
            builder.Entity<Cliente>(entity =>
            {
                entity.HasKey(e => e.ID_Cliente);
                entity.Property(e => e.name).IsRequired().HasMaxLength(255);
                entity.Property(e => e.address).IsRequired().HasMaxLength(255);
                entity.Property(e => e.phone).IsRequired().HasMaxLength(255);
                entity.Property(e => e.email).IsRequired().HasMaxLength(255);

                // Propiedades del contacto agregadas directamente a Cliente
                entity.Property(e => e.ContactoName).IsRequired(false).HasMaxLength(255);
                entity.Property(e => e.ContactoLastname).IsRequired(false).HasMaxLength(255);
                entity.Property(e => e.ContactoEmail).IsRequired(false).HasMaxLength(255);
                entity.Property(e => e.ContactoPhone).IsRequired(false).HasMaxLength(20);
            });

            // Configuración de la entidad Productos
            builder.Entity<Productos>(entity =>
            {
                entity.HasKey(e => e.ID_Productos);
                entity.Property(e => e.name).IsRequired().HasMaxLength(255);
                entity.Property(e => e.note).HasMaxLength(255);
                entity.Property(e => e.price).HasColumnType("DECIMAL(10, 2)");
            });

            // Configuración de la entidad Personal
            builder.Entity<Personal>(entity =>
            {
                entity.HasKey(e => e.ID_Personal);
                entity.Property(e => e.name).IsRequired().HasMaxLength(255);
                entity.Property(e => e.lastname).IsRequired().HasMaxLength(255);
                entity.Property(e => e.profession).IsRequired().HasMaxLength(255);
                entity.Property(e => e.salary).HasColumnType("DECIMAL(10, 2)");
                entity.Property(e => e.email).IsRequired().HasMaxLength(255);
                entity.Property(e => e.address).IsRequired().HasMaxLength(255);
                entity.Property(e => e.phone).IsRequired().HasMaxLength(255);
            });

            // Configuración de la entidad Cotizacion
            builder.Entity<Cotizacion>(entity =>
            {
                entity.HasKey(e => e.ID_Cotizacion);
                entity.HasOne(c => c.Cliente).WithMany().HasForeignKey(c => c.ID_Cliente);
                entity.Property(e => e.quotationDate).IsRequired().HasColumnType("DATE");
                entity.Property(e => e.name).IsRequired();
                entity.Property(e => e.quantityofproduct).IsRequired();
                entity.HasMany(c => c.ProductosCotizacion)
                      .WithOne(p => p.Cotizacion)
                      .HasForeignKey(p => p.ID_Cotizacion);
                entity.HasMany(c => c.PersonalCotizacion)
                      .WithOne(p => p.Cotizacion)
                      .HasForeignKey(p => p.ID_Cotizacion);
            });

            // Configuraciones para las relaciones de muchos a muchos
            builder.Entity<ProductoCotizacion>()
                .HasKey(pc => new { pc.ID_Cotizacion, pc.ID_Producto });

            builder.Entity<ProductoCotizacion>()
                .HasOne(pc => pc.Cotizacion)
                .WithMany(c => c.ProductosCotizacion)
                .HasForeignKey(pc => pc.ID_Cotizacion);

            builder.Entity<ProductoCotizacion>()
                .HasOne(pc => pc.Producto)
                .WithMany() // Aquí asumimos que no hay una propiedad de navegación en Productos hacia ProductoCotizacion
                .HasForeignKey(pc => pc.ID_Producto);

            builder.Entity<ProductoCotizacion>()
                .Property(pc => pc.Cantidad)
                .IsRequired();

            builder.Entity<PersonalCotizacion>()
                .HasKey(pc => new { pc.ID_Cotizacion, pc.ID_Personal });

            builder.Entity<PersonalCotizacion>()
                .HasOne(pc => pc.Cotizacion)
                .WithMany(c => c.PersonalCotizacion)
                .HasForeignKey(pc => pc.ID_Cotizacion);

            builder.Entity<PersonalCotizacion>()
                .HasOne(pc => pc.Personal)
                .WithMany() // Aquí asumimos que no hay una propiedad de navegación en Personal hacia PersonalCotizacion
                .HasForeignKey(pc => pc.ID_Personal);

        }
    }
}
