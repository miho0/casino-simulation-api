using CasinoSimulationApi.Models;
using CasinoSimulationApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CasinoSimulationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlackjackController : ControllerBase
    {
        private readonly BlackjackService _blackjackService;

        public BlackjackController(BlackjackService blackjackService)
        {
            _blackjackService = blackjackService;
        }

        [HttpGet("test")]
        public ActionResult<Decision> Test(int PlayerCardOne, int PlayerCardTwo, int DealerFaceUpCard)
        {
            return _blackjackService.GetDecision(PlayerCardOne, PlayerCardTwo, DealerFaceUpCard);
        }
    }
}
