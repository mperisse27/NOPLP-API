using Microsoft.EntityFrameworkCore;
using NOPLP_API.DTO;
using NOPLP_API.Data;

namespace NOPLP_API.Services
{
    public class GameService
    {
        async public Task<Game> GetNewGame(NoplpDbContext context)
        {
            var themeCount = await context.Themes.CountAsync();

            ICollection<Guid> usedThemes = new HashSet<Guid>();
            ICollection<GameCategory> categories = new List<GameCategory>();
            while (categories.Count < 5)
            {
                var randomIndex = new Random().Next(themeCount);

                var theme = await context.Themes
                    .Skip(randomIndex)
                    .FirstOrDefaultAsync();

                if (theme == null || usedThemes.Contains(theme.Id))
                {
                    continue;
                }

                var songs = await context.Songs
                    .Where(s => s.SongThemes.Any(st => st.ThemeId == theme.Id))
                    .Include(s => s.Artist)
                    .Include(s => s.SongThemes)
                        .ThenInclude(st => st.Theme)
                    .ToListAsync();

                usedThemes.Add(theme.Id);
                categories.Add(
                    new(
                        usedThemes.Count * 10,
                        theme.Name,
                        [
                            new(songs[0].Title, songs[0].Year, songs[0].Artist.Name),
                            new(songs[1].Title, songs[1].Year, songs[1].Artist.Name),
                        ]
                    )
                );
            }

            var memeChanson = await ChooseMemeChanson(context, categories);

            return new(memeChanson, categories);

        }

        async private Task<GameSong> ChooseMemeChanson(NoplpDbContext context, ICollection<GameCategory> categories)
        {
            var songCount = await context.Songs.CountAsync();

            var songIndex = new Random().Next(songCount);

            var memeChanson = await context.Songs
                .Skip(songIndex)
                .Include(s => s.Artist)
                .FirstOrDefaultAsync();


            return new(memeChanson.Title, memeChanson.Year, memeChanson.Artist.Name);
        }
    }
}
