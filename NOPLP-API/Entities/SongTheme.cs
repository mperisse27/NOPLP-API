namespace NOPLP_API.Entities
{
    public class SongTheme
    {
        public Guid SongId { get; set; }
        public Song Song { get; set; }

        public Guid ThemeId { get; set; }
        public Theme Theme { get; set; }
    }
}
