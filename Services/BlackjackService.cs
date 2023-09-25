using CasinoSimulationApi.Data;
using CasinoSimulationApi.Models;

namespace CasinoSimulationApi.Services
{
    public class BlackjackService
    {
        private readonly DecisionService _decisionService;

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

        public Player InitializePlayer(decimal StartingBalance, decimal BettingAmount)
        {
            return new Player(StartingBalance, BettingAmount);
        }

        public void PlayerLost(Player player)
        {
            player.CurrentBet *= 2;
        }

        public void PlayerWon(Player player)
        {
            player.Balance += player.CurrentBet * 2;
            player.CurrentBet = player.BettingAmount;
        }

        public void Tie(Player player)
        {
            player.Balance += player.CurrentBet;
        }

        // plays a round and return weather or not the player was able to play
        public bool PlayRound(Player player)
        {
            if (player.Balance < player.CurrentBet)
            {
                return false;
            }

            BlackjackGame game = GetNewGame();
            int playerTotal = game.PlayerCards.Sum();
            int dealerTotal = game.DealerFaceUpCard + game.DealerFaceDownCard;

            Decision decision = _decisionService.Decide(game);
            while (decision == Decision.Hit)
            {
                int newCard = GetNextCard();
                game.PlayerCards.Add(newCard);
                playerTotal += newCard;

                if (playerTotal > 21)
                {
                    break;
                }

                decision = _decisionService.Decide(game);
            }

            if (decision == Decision.Double)
            {
                game.PlayerCards.Add(GetNextCard());
                playerTotal = game.PlayerCards.Sum();
                player.Balance -= player.CurrentBet;
                player.CurrentBet *= 2;
            }

            if (playerTotal > 21)
            {
                PlayerLost(player);
                return true;
            }

            while (dealerTotal < 17)
            {
                dealerTotal += GetNextCard();
            }

            if (dealerTotal > 21 || playerTotal > dealerTotal)
            {
                PlayerWon(player);
                return true;
            }

            PlayerLost(player);
            return true;
        }

        public int PlayGame(decimal StartingBalance, decimal BettingAmount)
        {
            Player player = InitializePlayer(StartingBalance, BettingAmount);
            int totalRounds = 0;
            while (PlayRound(player))
            {
                totalRounds++;
            }
            return totalRounds;
        }
    }
}

// next: list of all games
// next: blackjack