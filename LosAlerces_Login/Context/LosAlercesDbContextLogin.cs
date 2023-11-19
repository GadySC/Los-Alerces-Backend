﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using LosAlerces_Login.Models;
using LosAlerces_Login.Entities;

namespace LosAlerces_Login.Context
{
    public class LosAlercesDbContextLogin : IdentityDbContext<ApplicationUser>
    {
        public LosAlercesDbContextLogin(DbContextOptions<LosAlercesDbContextLogin> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configuración de las tablas de Identity
            builder.Entity<IdentityUser>(entity => { entity.ToTable(name: "Usuarios"); });
            builder.Entity<IdentityRole>(entity => { entity.ToTable(name: "Roles"); });
            builder.Entity<IdentityUserRole<string>>(entity => { entity.ToTable("UserRoles"); });
            builder.Entity<IdentityUserClaim<string>>(entity => { entity.ToTable("UserClaims"); });
            builder.Entity<IdentityUserLogin<string>>(entity => { entity.ToTable("UserLogins"); });
            builder.Entity<IdentityRoleClaim<string>>(entity => { entity.ToTable("RoleClaims"); });
            builder.Entity<IdentityUserToken<string>>(entity => { entity.ToTable("UserTokens"); });

            builder.Entity<ApplicationUser>(entity =>
            {
                entity.HasIndex(u => u.Rut).IsUnique();
            });

            // Configuración de la relación entre Cliente y Contactos
            builder.Entity<Cliente>(entity =>
            {
                entity.HasKey(e => e.ID_Cliente);
                entity.Property(e => e.name).IsRequired().HasMaxLength(255);
                entity.Property(e => e.address).IsRequired().HasMaxLength(255);
                entity.Property(e => e.phone).IsRequired().HasMaxLength(255);
                entity.Property(e => e.email).IsRequired().HasMaxLength(255);
                entity.HasMany(c => c.Contactos).WithOne(e => e.Cliente).HasForeignKey(e => e.ID_Cliente);
            });

            builder.Entity<Contactos>(entity =>
            {
                entity.HasKey(e => e.ID_Contactos);
                entity.Property(e => e.name).IsRequired().HasMaxLength(255);
                entity.Property(e => e.lastname).IsRequired().HasMaxLength(255);
                entity.Property(e => e.email).IsRequired().HasMaxLength(255);
                entity.Property(e => e.phone).HasMaxLength(20);
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
                entity.Property(e => e.QuotationDate).IsRequired().HasColumnType("DATE");
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

            builder.Entity<PersonalCotizacion>()
                .HasKey(pc => new { pc.ID_Cotizacion, pc.ID_Personal });
        }
    }
}