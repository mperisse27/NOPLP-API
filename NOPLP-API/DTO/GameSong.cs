namespace NOPLP_API.DTO
{
    public class GameSong
    {
        public string Title { get; set; }
        public int Year { get; set; }

        public string ArtistName { get; set; }

        public GameSong(string title, int year, string artistName)
        {
            Title = title;
            Year = year;
            ArtistName = artistName;
        }
    }
}
