namespace NOPLP_API.Entities
{
    public class Theme
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public ICollection<SongTheme> SongThemes { get; set; } = new List<SongTheme>();
    }
}
