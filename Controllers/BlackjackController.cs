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
        [HttpGet("RandomDecisions")]
        public ActionResult<ProbabilityInformation> RandomDecisions(decimal InitialBalance, decimal BettingAmount, decimal Goal, int Itterations)
        {
            return _blackjackService.GetProbabilityInformation(InitialBalance, BettingAmount, Goal, Itterations, false, false);
        }

        [HttpGet("BasicStrategy")]
        public ActionResult<ProbabilityInformation> BasicStrategy(decimal InitialBalance, decimal BettingAmount, decimal Goal, int Itterations)
        {
            return _blackjackService.GetProbabilityInformation(InitialBalance, BettingAmount, Goal, Itterations, true, false);
        }
        [HttpGet("RandomDecisionsAndMartingale")]
        public ActionResult<ProbabilityInformation> RandomDecisionsAndMartingale(decimal InitialBalance, decimal BettingAmount, decimal Goal, int Itterations)
        {
            return _blackjackService.GetProbabilityInformation(InitialBalance, BettingAmount, Goal, Itterations, false);
        }

        [HttpGet("BasicStrategyAndMartingale")]
        public ActionResult<ProbabilityInformation> BasicStrategyAndMartingale(decimal InitialBalance, decimal BettingAmount, decimal Goal, int Itterations)
        {
            return _blackjackService.GetProbabilityInformation(InitialBalance, BettingAmount, Goal, Itterations);
        }
    }
}
