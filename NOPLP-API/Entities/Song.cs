namespace NOPLP_API.Entities
{
    public class Song
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public int Year { get; set; }
        public bool MemeChanson { get; set; }

        public Guid ArtistId { get; set; }
        public Artist Artist { get; set; }

        public ICollection<SongTheme> SongThemes { get; set; } = new List<SongTheme>();
    }
}
