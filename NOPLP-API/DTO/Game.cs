using System.ComponentModel;

namespace NOPLP_API.DTO
{
    public class Game
    {
        public GameSong MemeChanson { get; set; }
        public ICollection<GameCategory> Categories { get; set; }

        public Game(GameSong memeChanson, ICollection<GameCategory> categories)
        {
            MemeChanson = memeChanson;
            Categories = categories;
        }
    }
}
