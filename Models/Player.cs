namespace CasinoSimulationApi.Models
{
    public class Player
    {
        public decimal Balance { get; set; }
        public decimal BettingAmount { get; set; }
        public decimal Goal { get; set; }
        public decimal CurrentBet { get; set; }

        public Player(decimal balance, decimal bettingAmount, decimal goal)
        {
            Balance = balance;
            BettingAmount = bettingAmount;
            CurrentBet = bettingAmount;
            Goal = goal;
        }

        public bool CanBet()
        {
            if (Balance >= CurrentBet)
            {
                return true;
            }
            if (Balance == 0)
            {
                return false;
            }
            CurrentBet = Balance;
            return true;
        }

        public bool GoalReached()
        {
            return Balance >= Goal;
        }

        public void PlaceBet()
        {
            Balance -= CurrentBet;
        }

        public void Blackjack()
        {
            Balance += CurrentBet * 2.5m;
            CurrentBet = BettingAmount;
        }

        public void Lost()
        {
            CurrentBet *= 2;
        }

        public void Won()
        {
            Balance += CurrentBet * 2;
            CurrentBet = BettingAmount;
        }

        public void Tie()
        {
            Balance += CurrentBet;
        }

        public void Double()
        {
            Balance -= CurrentBet;
            CurrentBet *= 2;
        }
    }
}
