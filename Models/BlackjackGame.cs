namespace CasinoSimulationApi.Models
{
    public class BlackjackGame
    {
        public int DealerFaceUpCard { get; set; }
        public List<int> DealerCards { get; set; }
        public List<int> PlayerCards { get; set; }
        public bool SoftTotalPlayer { get; set; }
        public int AceIndexPlayer { get; set; }
        public bool IsPairPlayer { get; set; }
        public bool SoftTotalDealer { get; set; }
        public int AceIndexDealer { get; set; }

        public BlackjackGame(int dealerFaceUpCard, int dealerFaceDownCard, List<int> playerCards)
        {
            DealerFaceUpCard = dealerFaceUpCard;
            DealerCards = new List<int> { dealerFaceDownCard, dealerFaceUpCard };
            PlayerCards = playerCards;

            if (PlayerCards[0] == PlayerCards[1])
            {
                IsPairPlayer = true;
            }

            // if there is an ace in the player's hand, it is a soft total
            foreach(int card in playerCards)
            {
                if (card == 1)
                {
                    SoftTotalPlayer = true;
                    AceIndexPlayer = PlayerCards.IndexOf(card);
                }
            }

            // same goes for the dealer
            foreach (int card in DealerCards)
            {
                if (card == 1)
                {
                    SoftTotalDealer = true;
                    AceIndexDealer = DealerCards.IndexOf(card);
                }
            }


        }

        public void AddCardPlayer(int card)
        {
            PlayerCards.Add(card);
            if (!SoftTotalPlayer && card == 1)
            {
                SoftTotalPlayer = true;
                AceIndexPlayer = PlayerCards.IndexOf(card);
            }
        }

        public int CalculateTotalPlayer()
        {
            int total = PlayerCards.Sum();
            if (SoftTotalPlayer)
            {
                if (total + 10 <= 21 && PlayerCards[AceIndexPlayer] == 1)
                {
                    PlayerCards[AceIndexPlayer] = 11;
                    total += 10;
                }

                if (total > 21)
                {
                    PlayerCards[AceIndexPlayer] = 1;
                    total -= 10;
                }
            }
            return total;
        }

        public void AddCardDealer(int card)
        {
            DealerCards.Add(card);
            if (!SoftTotalDealer && card == 1)
            {
                SoftTotalDealer = true;
                AceIndexDealer = DealerCards.IndexOf(card);
            }
        }

        public int CalculateTotalDealer()
        {
            int total = DealerCards.Sum();
            if (SoftTotalDealer)
            {
                if (total + 10 <= 21 && DealerCards[AceIndexDealer] == 1)
                {
                    DealerCards[AceIndexDealer] = 11;
                    total += 10;
                }

                if (total > 21 && DealerCards[AceIndexDealer] == 1)
                {
                    DealerCards[AceIndexDealer] = 1;
                    total -= 10;
                }
            }
            return total;
        }

        public int CalculateTotalPlayerWithoutAce()
        {
            if (SoftTotalPlayer)
            {
                return PlayerCards.Sum() - PlayerCards[AceIndexPlayer];
            }

            return -1;
        }
    }
}

// TODO handle case where player has two or more aces