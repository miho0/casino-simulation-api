using CasinoSimulationApi.Models;

namespace CasinoSimulationApi.Services
{
    public class BlackjackService
    {
        
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

        public void PlayRound(Player player)
        {
            BlackjackGame game = GetNewGame();
            int playerTotal = game.PlayerCards.Sum();
            int dealerTotal = game.DealerFaceUpCard + game.DealerFaceDownCard;
            if (playerTotal > dealerTotal)
            {
                player.PreviousGameWon = true;
                player.Balance += player.CurrentBet;
                player.CurrentBet = player.BettingAmount;
            }
            else
            {
                player.PreviousGameWon = false;
                player.Balance -= player.CurrentBet;
                player.CurrentBet *= 2;
            }
        }

        public int PlayGame(decimal StartingBalance, decimal BettingAmount)
        {
            Player player = InitializePlayer(StartingBalance, BettingAmount);
            int totalRounds = 0;
            while (player.Balance > 0)
            {
                PlayRound(player);
                totalRounds++;
            }
            return totalRounds;
        }
    }
}
