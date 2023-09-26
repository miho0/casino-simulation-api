using CasinoSimulationApi.Data;
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
        private readonly DecisionService _decisionService;

        public BlackjackController(BlackjackService blackjackService, DecisionService decisionService)
        {
            _blackjackService = blackjackService;
            _decisionService = decisionService;
        }

        [HttpGet("test")]
        public ActionResult<List<BlackjackGameResult>> Test(decimal InitalBalance, decimal BettingAmount, decimal Goal)
        {
            return _blackjackService.PlayGame(InitalBalance, BettingAmount, Goal);
        }

        [HttpGet("test2")]
        public ActionResult<Decision> Test2(int dealerFaceUpCard, int p1, int p2)
        {
            BlackjackGame game = new BlackjackGame(dealerFaceUpCard, 10, new List<int> { p1, p2 });
            return _decisionService.Decide(game);
        }
    }
}
