namespace CasinoSimulationApi.Models
{
    public class BlackjackGame
    {
        public int DealerFaceUpCard { get; set; }
        public int DealerFaceDownCard { get; set; }
        public List<int> PlayerCards { get; set; }

        public BlackjackGame(int dealerFaceUpCard, int dealerFaceDownCard, List<int> playerCards)
        {
            DealerFaceUpCard = dealerFaceUpCard;
            DealerFaceDownCard = dealerFaceDownCard;
            PlayerCards = playerCards;
        }
    }
}
