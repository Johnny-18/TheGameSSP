using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RSPGame.Models;
using RSPGame.Services.Statistics;
using RSPGame.Storage;

namespace RSPGame.Controllers
{
    [ApiController]
    [Route("api/stat")]
    public class StatisticController : ControllerBase
    {
        private readonly IGeneralStatService _generalStat;

        private readonly IIndividualStatService _individualStat;

        private readonly IRspStorage _storage;

        public StatisticController(IIndividualStatService individualStat, IGeneralStatService generalStat,
            IRspStorage storage)
        {
            _individualStat = individualStat;
            _generalStat = generalStat;
            _storage = storage;
        }

        [HttpGet("general")]
        public async Task<IActionResult> GetGeneralStat()
        {
            var result = await _generalStat.GetStatAsync(_storage);
            if (result == null || result.Any() == false)
                return NoContent();

            return Ok(result);
        }

        [HttpPost("individual/{userName}")]
        public async Task<IActionResult> OnlineTime([FromRoute] string userName, [FromBody] TimeSpan onlineTime)
        {
            if (string.IsNullOrEmpty(userName) || onlineTime == TimeSpan.Zero)
                return BadRequest();

            var user = await _storage.GetUserByUserNameAsync(userName);
            _individualStat.ChangeOnlineTime(user.GamerInfo, onlineTime);

            return Ok();
        }
    }
}