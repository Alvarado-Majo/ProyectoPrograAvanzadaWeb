using CineStreamCR.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CineStreamCR.DAL.Data
{
    public class ProyectoDBContext: DbContext
    {
        public ProyectoDBContext()
        {
        }

        public ProyectoDBContext(DbContextOptions<ProyectoDBContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Actors> Actors { get; set; }
        public virtual DbSet<Directors> Directors { get; set; }
        public virtual DbSet<MovieActors> MovieActors { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=localhost;Database=CineStreamCR;User Id=sa;Password=your_password;");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Actors>(entity =>
            {
                entity.HasKey(e => e.ActorId);
                entity.Property(e => e.FirstName).IsRequired().HasColumnName("FirstName").HasMaxLength(100);
                entity.Property(e => e.LastName).IsRequired().HasColumnName("LastName").HasMaxLength(100);
                entity.Property(e =>e.Nationality).IsRequired().HasColumnName("Nationality").HasMaxLength(100);
                entity.Property(e => e.Biography).IsRequired().HasColumnName("Biography").HasMaxLength(1000);
                entity.Property(e => e.BirthDate).IsRequired().HasColumnName("BirthDate");
                entity.Property(e => e.PictureImg).IsRequired().HasColumnName("PictureImg").HasMaxLength(200);
                entity.Property(e => e.IsActive).IsRequired().HasColumnName("IsActive").HasDefaultValue(1);
            });
            modelBuilder.Entity<Directors>(entity =>
            {
                entity.HasKey(e => e.DirectorId);
                entity.Property(e => e.FirstName).IsRequired().HasColumnName("FirstName").HasMaxLength(100);
                entity.Property(e => e.LastName).IsRequired().HasColumnName("LastName").HasMaxLength(100);
                entity.Property(e =>e.Nationality).IsRequired().HasColumnName("Nationality").HasMaxLength(100);
                entity.Property(e => e.Biography).IsRequired().HasColumnName("Biography").HasMaxLength(1000);
                entity.Property(e => e.BirthDate).IsRequired().HasColumnName("BirthDate");
                entity.Property(e => e.PictureImg).IsRequired().HasColumnName("PictureImg").HasMaxLength(200);
                entity.Property(e => e.IsActive).IsRequired().HasColumnName("IsActive").HasDefaultValue(1);
            });
            modelBuilder.Entity<MovieActors>(entity =>
            {
                entity.HasKey(e => new { e.MovieId, e.ActorId });
                entity.Property(e => e.CharacterName).IsRequired().HasColumnName("CharacterName").HasMaxLength(100);
                entity.HasOne(e => e.Actors).WithMany().HasForeignKey(e => e.ActorId).HasConstraintName("FK_MovieActors_Actors");
                //Descomentar cuando este la entidad Movies creada
                //entity.HasOne(e => e.Movies).WithMany() .HasForeignKey(e => e.MovieId).HasConstraintName("FK_MovieActors_Movies");

            });
}
}
}

