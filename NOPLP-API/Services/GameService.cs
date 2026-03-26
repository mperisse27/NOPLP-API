using Microsoft.EntityFrameworkCore;
using NOPLP_API.Data;
using NOPLP_API.DTO;

namespace NOPLP_API.Services
{
    public class GameService
    {
        async public Task<Game> GetNewGame(NoplpDbContext context)
        {
            var random = new Random();
            var themeCount = await context.Themes.CountAsync();
            var artistCount = await context.Artists.Include(a => a.Songs).Where(a => a.Songs.Count > 1).CountAsync();
            var usedYearCat = false;
            ICollection<Guid> usedThemes = new HashSet<Guid>();
            ICollection<GameCategory> categories = new List<GameCategory>();
            while (categories.Count < 5)
            {
                var themeType = random.NextDouble();
                if (themeType < 0.15)
                {
                    var randomIndex = random.Next(artistCount);
                    var artist = await context.Artists.Include(a => a.Songs)
                        .Where(a => a.Songs.Count > 1)
                        .Skip(randomIndex)
                        .FirstOrDefaultAsync();

                    if (artist == null || usedThemes.Contains(artist.Id))
                    {
                        continue;
                    }

                    var songs = await context.Songs
                        .Where(s => s.ArtistId == artist.Id)
                        .ToListAsync();
                    songs = songs
                        .OrderBy(s => random.Next())
                        .ToList();
                    usedThemes.Add(artist.Id);
                    categories.Add(
                        new(
                            usedThemes.Count * 10,
                            artist.Name,
                            [
                                new(songs[0].Title, songs[0].Year, songs[0].Artist.Name),
                                new(songs[1].Title, songs[1].Year, songs[1].Artist.Name),
                            ]
                        )
                    );
                }
                else if (themeType < 0.3 && !usedYearCat)
                {
                    List<string> yearCategories =
                    [
                        "Les années 70",
                        "Les années 80",
                        "Les années 90",
                        "Les années 2000",
                        "Les années 2010",
                    ];
                    var randomIndex = random.Next(yearCategories.Count);
                    var minYear = 1970 + randomIndex * 10;

                    var songs = await context.Songs
                        .Where(s => s.Year >= minYear && s.Year < minYear + 10)
                        .Include(s => s.Artist)
                        .ToListAsync();
                    songs = songs
                        .OrderBy(s => random.Next())
                        .ToList();
                    usedYearCat = true;
                    usedThemes.Add(new Guid());
                    categories.Add(
                        new(
                            usedThemes.Count * 10,
                            yearCategories[randomIndex],
                            [
                                new(songs[0].Title, songs[0].Year, songs[0].Artist.Name),
                                new(songs[1].Title, songs[1].Year, songs[1].Artist.Name),
                            ]
                        )
                    );
                }
                else
                {
                    var randomIndex = random.Next(themeCount);

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
                    songs = songs
                        .OrderBy(s => random.Next())
                        .ToList();
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
