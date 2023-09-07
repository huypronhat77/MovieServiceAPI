using Microsoft.EntityFrameworkCore;

namespace MovieServiceWebAPI.Data
{
    public class MyDBContext : DbContext
    {
        public MyDBContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            modelBuilder.Entity<Movie>(m => {
                m.ToTable("Movie");
                m.HasKey(k => k.Id);
                m.Property(m => m.Description).HasMaxLength(250);
            });

            modelBuilder.Entity<MovieGenre>(mg => {
                mg.HasKey(k => new { k.MovieId, k.GenreId });

                mg.HasOne(mg => mg.Movie)
                    .WithMany(m => m.MovieGenres)
                    .HasForeignKey(mg => mg.MovieId);

                mg.HasOne(mg => mg.Genre)
                    .WithMany(g => g.MovieGenres)
                    .HasForeignKey(mg => mg.GenreId);
            });
        }

        #region DbSet
        public  DbSet<Movie> Movies { get; set;}
        public DbSet<Genre> Genres { get; set;}
        public DbSet<MovieGenre> MovieGenres { get; set;}
        #endregion
    }
}
