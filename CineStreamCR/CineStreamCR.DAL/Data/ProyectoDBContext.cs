using CineStreamCR.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CineStreamCR.DAL.Data
{
    public partial class ProyectoDBContext : DbContext
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

        // Tablas intermedias para relaciones muchos a muchos
        public virtual DbSet<Categories> Categories { get; set; }
        public virtual DbSet<MovieCategories> MovieCategories { get; set; }
        public virtual DbSet<MovieDirectors> MovieDirectors { get; set; }
        public virtual DbSet<WatchLists> WatchLists { get; set; }
        public virtual DbSet<WatchListMovies> WatchListMovies { get; set; }
        public virtual DbSet<Reviews> Reviews { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // La cadena de conexión se configura mediante Dependency Injection desde Program.cs y appsettings.json.
                // Comentario: Javier Méndez González
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Actors

            modelBuilder.Entity<Actors>(entity =>
            {
                entity.ToTable("Actors");

                entity.HasKey(e => e.ActorId);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Nationality)
                    .HasMaxLength(100);

                entity.Property(e => e.Biography);

                entity.Property(e => e.BirthDate);

                entity.Property(e => e.PictureImg)
                    .HasMaxLength(255);

                entity.Property(e => e.IsActive)
                    .HasDefaultValue((byte)1);
            });

            #endregion

            #region Directors

            modelBuilder.Entity<Directors>(entity =>
            {
                entity.ToTable("Directors");

                entity.HasKey(e => e.DirectorId);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Nationality)
                    .HasMaxLength(100);

                entity.Property(e => e.Biography);

                entity.Property(e => e.BirthDate);

                entity.Property(e => e.PictureImg)
                    .HasMaxLength(255);

                entity.Property(e => e.IsActive)
                    .HasDefaultValue((byte)1);
            });

            #endregion

            #region Movies

            modelBuilder.Entity<Movies>(entity =>
            {
                entity.ToTable("Movies");

                entity.HasKey(e => e.MovieId);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.MovieRating)
                    .HasColumnType("decimal(3,1)");

                entity.Property(e => e.Synopsis);

                entity.Property(e => e.ReleaseYear);

                entity.Property(e => e.DurationMinutes);

                entity.Property(e => e.PosterImg)
                    .HasMaxLength(255);

                entity.Property(e => e.VideoUrl)
                    .HasMaxLength(255);

                entity.Property(e => e.Nationality)
                    .HasMaxLength(70);

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("GETDATE()");

                entity.Property(e => e.UpdatedAt);

                entity.Property(e => e.IsActive)
                    .HasDefaultValue((byte)1);

                // DirectorId eliminado.
                // La relación ahora será mediante MovieDirectors.
            });

            #endregion

            #region MovieActors

            modelBuilder.Entity<MovieActors>(entity =>
            {
                entity.ToTable("MovieActors");

                entity.HasKey(e => new { e.MovieId, e.ActorId });

                entity.Property(e => e.CharacterName)
                    .HasMaxLength(100);

                entity.HasOne(e => e.Movie)
                    .WithMany(m => m.MovieActors)
                    .HasForeignKey(e => e.MovieId);

                entity.HasOne(e => e.Actor)
                    .WithMany(a => a.MovieActors)
                    .HasForeignKey(e => e.ActorId);
            });

            #endregion

            #region Users

            modelBuilder.Entity<Users>(entity =>
            {
                entity.ToTable("Users");

                entity.HasKey(e => e.UserId);

                entity.HasIndex(e => e.Email)
                    .IsUnique();

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.PasswordHash)
                    .IsRequired();

                entity.Property(e => e.PasswordSalt)
                    .IsRequired();

                entity.Property(e => e.SignUpDate)
                    .HasDefaultValueSql("GETDATE()");

                entity.Property(e => e.IsActive)
                    .HasDefaultValue((byte)1);
            });

            #endregion

            #region Categories

            modelBuilder.Entity<Categories>(entity =>
            {
                entity.ToTable("Categories");

                entity.HasKey(e => e.CategoryId);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasIndex(e => e.Name)
                    .IsUnique();
            });

            #endregion

            #region MovieCategories

            modelBuilder.Entity<MovieCategories>(entity =>
            {
                entity.ToTable("MovieCategories");

                entity.HasKey(e => new
                {
                    e.MovieId,
                    e.CategoryId
                });

                entity.HasOne(e => e.Movie)
                    .WithMany(m => m.MovieCategories)
                    .HasForeignKey(e => e.MovieId);

                entity.HasOne(e => e.Category)
                    .WithMany(c => c.MovieCategories)
                    .HasForeignKey(e => e.CategoryId);
            });

            #endregion

            #region MovieDirectors

            modelBuilder.Entity<MovieDirectors>(entity =>
            {
                entity.ToTable("MovieDirectors");

                entity.HasKey(e => new
                {
                    e.MovieId,
                    e.DirectorId
                });

                entity.HasOne(e => e.Movie)
                    .WithMany(m => m.MovieDirectors)
                    .HasForeignKey(e => e.MovieId);

                // Check abajo y corregir

                entity.HasOne(e => e.Director)
                    .WithMany(d => d.MovieDirectors)
                    .HasForeignKey(e => e.DirectorId);
            });

            #endregion

            #region WatchLists

            modelBuilder.Entity<WatchLists>(entity =>
            {
                entity.ToTable("WatchLists");

                entity.HasKey(e => e.WatchListId);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Description)
                    .HasMaxLength(255);

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("GETDATE()");

                entity.HasOne(e => e.User)
                    .WithMany(u => u.WatchLists)
                    .HasForeignKey(e => e.UserId);
            });

            #endregion


            #region WatchListMovies

            modelBuilder.Entity<WatchListMovies>(entity =>
            {
                entity.ToTable("WatchListMovies");

                entity.HasKey(e => new
                {
                    e.WatchListId,
                    e.MovieId
                });

                entity.HasOne(e => e.WatchList)
                    .WithMany(w => w.WatchListMovies)
                    .HasForeignKey(e => e.WatchListId);

                entity.HasOne(e => e.Movie)
                    .WithMany(m => m.WatchListMovies)
                    .HasForeignKey(e => e.MovieId);
            });

            #endregion


            #region Reviews

            modelBuilder.Entity<Reviews>(entity =>
            {
                entity.ToTable("Reviews");

                entity.HasKey(e => e.ReviewId);

                entity.Property(e => e.IsLike)
                    .IsRequired();

                entity.Property(e => e.ReviewDate)
                    .HasDefaultValueSql("GETDATE()");

                entity.HasOne(e => e.User)
                    .WithMany(u => u.Reviews)
                    .HasForeignKey(e => e.UserId);

                entity.HasOne(e => e.Movie)
                    .WithMany(m => m.Reviews)
                    .HasForeignKey(e => e.MovieId);
            });

            #endregion




            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}