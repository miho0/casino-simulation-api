using CasinoSimulationApi.Data;
using CasinoSimulationApi.Models;

namespace CasinoSimulationApi.Services
{
    public class BlackjackService
    {
        private readonly DecisionService _decisionService;
        public List<BlackjackGameResult> Results { get; set; } = new List<BlackjackGameResult>();

        public BlackjackService(DecisionService decisionService)
        {
            _decisionService = decisionService;
        }


        // returns a new BlackjackGame object, which contains all cards for the game
        public BlackjackGame GetNewGame(BlackjackGame previousGame = null)
        {
            if (previousGame != null)
            {;
                return new BlackjackGame(previousGame);

            } else
            {
                return new BlackjackGame();
            }
        }

        // plays a round and return weather or not the player was able to play
        public BlackjackGameResult PlayRound(BlackjackGame game, Player player)
        {
            player.PlaceBet();

            int playerTotal = game.CalculateTotalPlayer();
            int dealerTotal = game.CalculateTotalDealer();

            decimal bet = player.CurrentBet;
            Decision initialDecision = Decision.None;

            if (playerTotal == 21)
            {
                if (dealerTotal == 21)
                {
                    player.Tie();
                    return new BlackjackGameResult(Result.Push, bet, initialDecision, player.Balance, game.PlayerCards, game.DealerCards, playerTotal, dealerTotal);
                } else
                {
                    player.Blackjack();
                    return new BlackjackGameResult(Result.Blackjack, bet, initialDecision, player.Balance, game.PlayerCards, game.DealerCards, playerTotal, dealerTotal);
                }
            }

            if (dealerTotal == 21)
            {
                player.Lost();
                return new BlackjackGameResult(Result.DealerWon, bet, initialDecision, player.Balance, game.PlayerCards, game.DealerCards, playerTotal, dealerTotal);
            }

            Decision decision = _decisionService.Decide(game);
            initialDecision = decision;

            bool hasHit = false;

            if (decision == Decision.Split)
            {
                if (player.CanBet() && game.CanSplit())
                {
                    game.Split();
                    decision = _decisionService.Decide(game);
                    BlackjackGame newGame = GetNewGame(game);
                    Results.Add(PlayRound(newGame, player));
                    
                } else
                {
                    decision = _decisionService.Decide(game);
                }
            }

            if (decision == Decision.Double)
            {
                if (player.CanBet())
                {
                    player.Double();
                    game.AddCardPlayer();
                    decision = Decision.Stand;
                } else
                {
                    decision = Decision.Hit;
                }

            }

            while (decision == Decision.Hit || (decision == Decision.Double && hasHit))
            {
                game.AddCardPlayer();
                playerTotal = game.CalculateTotalPlayer();

                if (playerTotal > 21)
                {
                    player.Lost();
                    return new BlackjackGameResult(Result.PlayerBusted, bet, initialDecision, player.Balance, game.PlayerCards, game.DealerCards, playerTotal, dealerTotal);
                }

                decision = _decisionService.Decide(game);
                hasHit = true;
            }

            playerTotal = game.CalculateTotalPlayer();

            while (dealerTotal < 17)
            {
                game.AddCardDealer();
                dealerTotal = game.CalculateTotalDealer();
            }

            if (dealerTotal > 21)
            {
                player.Won();
                return new BlackjackGameResult(Result.DealerBusted, bet, initialDecision, player.Balance, game.PlayerCards, game.DealerCards, playerTotal, dealerTotal);
            }

            if (playerTotal > dealerTotal)
            {
                player.Won();
                return new BlackjackGameResult(Result.PlayerWon, bet, initialDecision, player.Balance, game.PlayerCards, game.DealerCards, playerTotal, dealerTotal);
            }

            if (playerTotal == dealerTotal)
            {
                player.Tie();
                return new BlackjackGameResult(Result.Push, bet, initialDecision, player.Balance, game.PlayerCards, game.DealerCards, playerTotal, dealerTotal);
            }

            if (dealerTotal > playerTotal)
            {
                player.Lost();
                return new BlackjackGameResult(Result.DealerWon, bet, initialDecision, player.Balance, game.PlayerCards, game.DealerCards, playerTotal, dealerTotal);
            }
            throw new Exception("Something went wrong");
        }

        public List<BlackjackGameResult> GetGameResults(decimal StartingBalance, decimal BettingAmount, decimal Goal)
        {
            Player player = new Player(StartingBalance, BettingAmount, Goal);
            while (player.CanBet() && !player.GoalReached()) {
                BlackjackGame game = GetNewGame();
                Results.Add(PlayRound(game, player));
            }
            return Results;
        }

        public ProbabilityInformation GetProbabilityInformation(decimal StartingBalance, decimal BettingAmount, decimal Goal, int Itterations)
        {
            int successes = 0;
            int roundsPlayed = 0;
            for (int i = 0; i < Itterations; i++)
            {
                Player player = new Player(StartingBalance, BettingAmount, Goal);
                while (player.CanBet() && !player.GoalReached())
                {
                    BlackjackGame game = GetNewGame();
                    Results.Add(PlayRound(game, player));
                    roundsPlayed++;
                }

                if (player.CanBet())
                {
                    successes++;
                }
            }
            return new ProbabilityInformation { 
                Probability = ((decimal) successes / Itterations) * 100,
                AverageRoundsPlayed = roundsPlayed / Itterations
            };
        }
    }
}