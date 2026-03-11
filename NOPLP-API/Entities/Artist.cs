namespace NOPLP_API.Entities
{
    public class Artist
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public ICollection<SongTheme> SongThemes { get; set; } = new List<SongTheme>();
    }
}
