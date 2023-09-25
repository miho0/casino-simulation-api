namespace CasinoSimulationApi.Models
{
    public class Player
    {
        public decimal Balance { get; set; }
        public decimal BettingAmount { get; set; }
        public decimal CurrentBet { get; set; }
        public bool PreviousGameWon { get; set; }

        public Player(decimal balance, decimal bettingAmount)
        {
            Balance = balance;
            BettingAmount = bettingAmount;
        }
    }
}
