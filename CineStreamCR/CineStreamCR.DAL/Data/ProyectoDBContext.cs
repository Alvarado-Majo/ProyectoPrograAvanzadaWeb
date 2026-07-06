using CineStreamCR.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CineStreamCR.DAL.Data
{
    public class ProyectoDBContext : DbContext
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
        public virtual DbSet<Movies> Movies { get; set; }
        public virtual DbSet<Users> Users { get; set; }
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
                entity.Property(e => e.Nationality).IsRequired().HasColumnName("Nationality").HasMaxLength(100);
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
                entity.Property(e => e.Nationality).IsRequired().HasColumnName("Nationality").HasMaxLength(100);
                entity.Property(e => e.Biography).IsRequired().HasColumnName("Biography").HasMaxLength(1000);
                entity.Property(e => e.BirthDate).IsRequired().HasColumnName("BirthDate");
                entity.Property(e => e.PictureImg).IsRequired().HasColumnName("PictureImg").HasMaxLength(200);
                entity.Property(e => e.IsActive).IsRequired().HasColumnName("IsActive").HasDefaultValue(1);
            });
            modelBuilder.Entity<Movies>(entity =>
            {
                entity.HasKey(e => e.MovieId);

                entity.Property(e => e.Title).IsRequired().HasColumnName("Title").HasMaxLength(150);

                entity.Property(e => e.Synopsis).IsRequired().HasColumnName("Synopsis").HasMaxLength(1000);

                entity.Property(e => e.ReleaseYear).IsRequired().HasColumnName("ReleaseYear");

                entity.Property(e => e.DurationMinutes).IsRequired().HasColumnName("DurationMinutes");

                entity.Property(e => e.Rating).IsRequired().HasColumnName("Rating").HasColumnType("decimal(3,1)");

                entity.Property(e => e.PosterImg).IsRequired().HasColumnName("PosterImg").HasMaxLength(200);

                entity.Property(e => e.VideoUrl).HasColumnName("VideoUrl").HasMaxLength(300);

                entity.Property(e => e.IsActive).IsRequired().HasColumnName("IsActive").HasDefaultValue((byte)1);

                entity.Property(e => e.DirectorId).IsRequired().HasColumnName("DirectorId");

                entity.HasOne(e => e.Director).WithMany().HasForeignKey(e => e.DirectorId).HasConstraintName("FK_Movies_Directors");
            });

            modelBuilder.Entity<MovieActors>(entity =>
            {
                entity.HasKey(e => new { e.MovieId, e.ActorId });

                entity.Property(e => e.CharacterName).IsRequired().HasColumnName("CharacterName").HasMaxLength(100);

                entity.HasOne(e => e.Actors).WithMany(a => a.MovieActors).HasForeignKey(e => e.ActorId).HasConstraintName("FK_MovieActors_Actors");

                entity.HasOne(e => e.Movie).WithMany(m => m.MovieActors).HasForeignKey(e => e.MovieId).HasConstraintName("FK_MovieActors_Movies");
            });


            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.FirstName).IsRequired().HasColumnName("FirstName").HasMaxLength(50);

                entity.Property(e => e.LastName).IsRequired().HasColumnName("LastName").HasMaxLength(50);

                entity.Property(e => e.Email).IsRequired().HasColumnName("Email").HasMaxLength(150);

                entity.HasIndex(e => e.Email).IsUnique();

                entity.Property(e => e.PasswordHash).IsRequired().HasColumnName("PasswordHash");

                entity.Property(e => e.PasswordSalt).IsRequired().HasColumnName("PasswordSalt");

                entity.Property(e => e.SignUpDate).IsRequired().HasColumnName("SignUpDate").HasDefaultValueSql("GETDATE()");

                entity.Property(e => e.IsActive).IsRequired().HasColumnName("IsActive").HasDefaultValue((byte)1);
            });
        }
    }
}

