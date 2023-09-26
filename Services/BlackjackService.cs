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
        
        // returns the next card value
        public int GetNextCard()
        {
            Random rand = new Random();
            int num = rand.Next(1, 14);
            if (num > 10)
            {
                return 10;
            }
            else
            {
                return num;
            }
        } 


        // returns a new BlackjackGame object, which contains all cards for the game
        public BlackjackGame GetNewGame()
        {
            List<int> playerCards = new List<int>();
            playerCards.Add(GetNextCard());
            int dealerFaceDownCard = GetNextCard();
            playerCards.Add(GetNextCard());
            int dealerFaceUpCard = GetNextCard();
            return new BlackjackGame(dealerFaceUpCard, dealerFaceDownCard, playerCards);
        }

        // plays a round and return weather or not the player was able to play
        public BlackjackGameResult PlayRound(Player player)
        {
            player.PlaceBet();
            BlackjackGame game = GetNewGame();

            int playerTotal = game.CalculateTotalPlayer();
            int dealerTotal = game.CalculateTotalDealer();

            decimal bet = player.CurrentBet;
            bool isDouble = false;


            if (playerTotal == 21)
            {
                if (dealerTotal == 21)
                {
                    player.Tie();
                    return new BlackjackGameResult(Result.Push, bet, isDouble, player.Balance, game.PlayerCards, game.DealerCards, playerTotal, dealerTotal);
                } else
                {
                    player.Blackjack();
                    return new BlackjackGameResult(Result.Blackjack, bet, isDouble, player.Balance, game.PlayerCards, game.DealerCards, playerTotal, dealerTotal);
                }
            }

            if (dealerTotal == 21)
            {
                player.Lost();
                return new BlackjackGameResult(Result.DealerWon, bet, isDouble, player.Balance, game.PlayerCards, game.DealerCards, playerTotal, dealerTotal);
            }

            Decision decision = _decisionService.Decide(game);
            bool hasHit = false;

            while (decision == Decision.Hit || (decision == Decision.Double && hasHit))
            {
                int newCard = GetNextCard();
                game.PlayerCards.Add(newCard);
                playerTotal += newCard;

                if (playerTotal > 21)
                {
                    player.Lost();
                    return new BlackjackGameResult(Result.PlayerBusted, bet, isDouble, player.Balance, game.PlayerCards, game.DealerCards, playerTotal, dealerTotal);
                }

                decision = _decisionService.Decide(game);
                hasHit = true;
            }

            if (decision == Decision.Double)
            {
                player.Double();
                isDouble = true;
                int newCard = GetNextCard();
                game.AddCardPlayer(newCard);
            }

            playerTotal = game.CalculateTotalPlayer();

            while (dealerTotal < 17)
            {
                int newCard = GetNextCard();
                game.AddCardDealer(newCard);
                dealerTotal = game.CalculateTotalDealer();
            }

            if (dealerTotal > 21)
            {
                player.Won();
                return new BlackjackGameResult(Result.DealerBusted, bet, isDouble, player.Balance, game.PlayerCards, game.DealerCards, playerTotal, dealerTotal);
            }

            if (playerTotal > dealerTotal)
            {
                player.Won();
                return new BlackjackGameResult(Result.PlayerWon, bet, isDouble, player.Balance, game.PlayerCards, game.DealerCards, playerTotal, dealerTotal);
            }

            if (playerTotal == dealerTotal)
            {
                player.Tie();
                return new BlackjackGameResult(Result.Push, bet, isDouble, player.Balance, game.PlayerCards, game.DealerCards, playerTotal, dealerTotal);
            }

            if (dealerTotal > playerTotal)
            {
                player.Lost();
                return new BlackjackGameResult(Result.DealerWon, bet, isDouble, player.Balance, game.PlayerCards, game.DealerCards, playerTotal, dealerTotal);
            }
            throw new Exception("Something went wrong");
        }

        public List<BlackjackGameResult> PlayGame(decimal StartingBalance, decimal BettingAmount)
        {
            Player player = new Player(StartingBalance, BettingAmount);
            while (player.CanBet()) {
                Results.Add(PlayRound(player));
            }
            return Results;
        }
    }
}