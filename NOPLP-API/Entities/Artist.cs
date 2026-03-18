namespace NOPLP_API.Entities
{
    public class Artist
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public ICollection<Song> Songs { get; set; } = new List<Song>();
    }
}
