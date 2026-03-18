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

        [HttpGet("newgame")]
        public async Task<IActionResult> GetNewGame()
        {
            var gameService = new GameService();
            return Ok(await gameService.GetNewGame(_context));
        }
    }
}
