using Microsoft.EntityFrameworkCore;
using NOPLP_API.DTO;
using NOPLP_API.Entities;

namespace NOPLP_API.Data
{
    public class NoplpDbContext : DbContext
    {
        public DbSet<Song> Songs { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Theme> Themes { get; set; }
        public DbSet<SongTheme> SongThemes { get; set; }

        public NoplpDbContext(DbContextOptions<NoplpDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SongTheme>()
                .HasKey(st => new { st.SongId, st.ThemeId });

            modelBuilder.Entity<SongTheme>()
                .HasOne(st => st.Song)
                .WithMany(s => s.SongThemes)
                .HasForeignKey(st => st.SongId);

            modelBuilder.Entity<SongTheme>()
                .HasOne(st => st.Theme)
                .WithMany(t => t.SongThemes)
                .HasForeignKey(st => st.ThemeId);

            modelBuilder.Entity<Song>().ToTable("songs");
            modelBuilder.Entity<Song>()
                .Property(s => s.Id).HasColumnName("id");
            modelBuilder.Entity<Song>()
                .Property(s => s.Title).HasColumnName("title");
            modelBuilder.Entity<Song>()
                .Property(s => s.Year).HasColumnName("year");
            modelBuilder.Entity<Song>()
                .Property(s => s.ArtistId).HasColumnName("artist_id");
            modelBuilder.Entity<Song>()
                .Property(s => s.MemeChanson).HasColumnName("meme_chanson");


            modelBuilder.Entity<Artist>().ToTable("artists");
            modelBuilder.Entity<Artist>()
                .Property(s => s.Id).HasColumnName("id");
            modelBuilder.Entity<Artist>()
                .Property(s => s.Name).HasColumnName("name");

            modelBuilder.Entity<Theme>().ToTable("themes");
            modelBuilder.Entity<Theme>()
                .Property(s => s.Id).HasColumnName("id");
            modelBuilder.Entity<Theme>()
                .Property(s => s.Name).HasColumnName("name");

            modelBuilder.Entity<SongTheme>().ToTable("songs_themes");
            modelBuilder.Entity<SongTheme>()
                .Property(s => s.SongId).HasColumnName("song_id");
            modelBuilder.Entity<SongTheme>()
                .Property(s => s.ThemeId).HasColumnName("theme_id");
        }
    }
}