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

        [HttpGet("GetGameResults")]
        public ActionResult<List<BlackjackGameResult>> GetGameResults(decimal InitialBalance, decimal BettingAmount, decimal Goal)
        {
            return _blackjackService.GetGameResults(InitialBalance, BettingAmount, Goal);
        }

        [HttpGet("GetProbability")]
        public ActionResult<ProbabilityInformation> GetProbability(decimal InitialBalance, decimal BettingAmount, decimal Goal, int Itterations)
        {
            return _blackjackService.GetProbabilityInformation(InitialBalance, BettingAmount, Goal, Itterations);
        }

        [HttpGet("testDecision")]
        public ActionResult<Decision> TestDecision(int d1, int d2, int p1, int p2)
        {
            BlackjackGame game = new BlackjackGame(new List<int> { d1, d2 }, new List<int> { p1, p2 });
            return _decisionService.Decide(game);
        }
    }
}
