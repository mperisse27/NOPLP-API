using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NOPLP_API.Data;
using NOPLP_API.DTO;
using NOPLP_API.Services;

namespace NOPLP_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameController : ControllerBase
    {
        private readonly NoplpDbContext _context;
        public GameController(NoplpDbContext context)
        {
            _context = context;
        }

        [HttpGet("random")]
        public async Task<IActionResult> Random()
        {
            var themeCount = await _context.Themes.CountAsync();

            if (themeCount == 0)
                return NotFound();

            ICollection<Guid> usedThemes = new List<Guid>();
            ICollection<GameCategory> categories = new List<GameCategory>();
            while (categories.Count < 5)
            {
                var randomIndex = new Random().Next(themeCount);

                var theme = await _context.Themes
                    .Skip(randomIndex)
                    .FirstOrDefaultAsync();

                if (theme == null || usedThemes.Contains(theme.Id))
                {
                    continue;
                }

                var songs = await _context.Songs
                    .Where(s => s.SongThemes.Any(st => st.ThemeId == theme.Id))
                    .Include(s => s.Artist)
                    .Include(s => s.SongThemes)
                        .ThenInclude(st => st.Theme)
                    .ToListAsync();
                
                usedThemes.Add(theme.Id);
                categories.Add(new(usedThemes.Count * 10, theme.Name, []));

            }

            //Choose Meme chanson
            var songCount = await _context.Themes.CountAsync();

            if (songCount == 0)
                return NotFound();

            var songIdx = new Random().Next(songCount);

            var memeChanson = await _context.Songs
                .Skip(songIdx)
                .FirstOrDefaultAsync();

            //Create final object
            Game newGame = new(new(memeChanson.Title, memeChanson.Year, memeChanson.Artist.Name), categories);

            return Ok(newGame);
        }

        [HttpGet("newgame")]
        public async Task<IActionResult> GetNewGame()
        {
            var gameService = new GameService();
            return Ok(await gameService.GetNewGame(_context));
        }
    }
}
