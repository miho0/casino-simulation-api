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
        public bool IsSplitted { get; set; }

        public BlackjackGame()
        {
            InitGame();

            if (PlayerCards[0] == PlayerCards[1])
            {
                IsPairPlayer = true;
            }

            IsSplitted = false;
        }

        // when the player has split, some data has to be copied from the original game
        public BlackjackGame(BlackjackGame previousGame)
        {
            DealerFaceUpCard = previousGame.DealerFaceUpCard;
            DealerCards = previousGame.DealerCards;
            PlayerCards = new List<int>{ previousGame.PlayerCards[0]} ;
            if (PlayerCards[0] == 1)
            {
                SoftTotalPlayer = true;
                AceIndexPlayer = 0;
            }

            if (DealerCards[0] == 1)
            {
                SoftTotalDealer = true;
                AceIndexDealer = 0;
            }
            IsSplitted = true;
        }

        public void InitGame()
        {
            AddCardPlayer();
            AddCardDealer();
            AddCardPlayer();
            AddCardDealer(); // has to be done in this order
        }

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

        public void AddCardPlayer()
        {
            if (PlayerCards == null)
            {
                PlayerCards = new List<int>();
            }
            int card = GetNextCard();
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

        public void AddCardDealer()
        {
            int card = GetNextCard();
            if (DealerCards == null)
            {
                DealerCards = new List<int>();
                DealerFaceUpCard = card;
            }
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

        public bool CanSplit()
        {
            return IsSplitted == false && IsPairPlayer;
        }

        public void Split()
        {
            IsSplitted = true;
            PlayerCards.Remove(1);
            AddCardPlayer();
        }
    }
}

// TODO handle case where player has two or more aces