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
        public void PlayRound(Player player)
        {
            player.PlaceBet();
            bool gameOver = false;

            BlackjackGame game = GetNewGame();
            BlackjackGameResult result = new BlackjackGameResult();

            int playerTotal = game.PlayerCards.Sum();
            int dealerTotal = game.DealerFaceUpCard + game.DealerFaceDownCard;
            List<int> dealerCards = new List<int> { game.DealerFaceDownCard, game.DealerFaceUpCard };
            Result gameResult = Result.Blackjack;

            result.PlayerTotal = playerTotal;
            result.Bet = player.CurrentBet;

            if (playerTotal == 21)
            {
                player.Blackjack();
                gameResult = Result.Blackjack;
                gameOver = true;
            }

            Decision decision = _decisionService.Decide(game);
            while (decision == Decision.Hit)
            {
                int newCard = GetNextCard();
                game.PlayerCards.Add(newCard);
                playerTotal += newCard;
                result.PlayerTotal = playerTotal;

                if (playerTotal > 21 && !gameOver)
                {
                    player.Lost();
                    gameResult = Result.PlayerBusted;
                    gameOver = true;
                }

                decision = _decisionService.Decide(game);
            }

            if (decision == Decision.Double)
            {
                player.PlaceBet();
                result.Double = true;
                int newCard = GetNextCard();
                game.PlayerCards.Add(newCard);
                playerTotal += newCard;
                player.Double();
            }

            while (dealerTotal < 17 && !gameOver)
            {
                int newCard = GetNextCard();
                dealerTotal += newCard;
                dealerCards.Add(newCard);
            }

            if (dealerTotal > 21 && !gameOver)
            {
                player.Won();
                gameResult = Result.DealerBusted;
                gameOver = true;
            }

            if (playerTotal > dealerTotal && !gameOver)
            {
                player.Won();
                gameResult = Result.PlayerWon;  
                gameOver = true;
            }

            if (playerTotal == dealerTotal && !gameOver)
            {
                player.Tie();
                gameResult = Result.Push;
                gameOver = true;
            }

            if (dealerTotal > playerTotal && !gameOver)
            {
                player.Lost();
                gameResult = Result.DealerWon;
            }

            result.PlayerCards = game.PlayerCards;
            result.DealerCards = dealerCards;
            result.DealerTotal = dealerTotal;
            result.EndBalance = player.Balance;
            result.SetResult(gameResult);
            Results.Add(result);
        }

        public List<BlackjackGameResult> PlayGame(decimal StartingBalance, decimal BettingAmount)
        {
            Player player = new Player(StartingBalance, BettingAmount);
            while (player.CanBet()) {
                PlayRound(player);
            }
            return Results;
        }
    }
}

// next: list of all games
// next: blackjack