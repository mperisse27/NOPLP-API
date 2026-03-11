namespace NOPLP_API.DTO
{
    public class GameCategory
    {
        public int Points { get; set; }
        public string Name { get; set; }
        public ICollection<GameSong> Songs { get; set; }

        public GameCategory(int points, string name, ICollection<GameSong> songs)
        {
            Points = points;
            Name = name;
            Songs = songs;
        }
    }
}
