﻿namespace CasinoSimulationApi.Models
{
    public enum Result
    {
        Blackjack,
        PlayerWon,
        DealerWon,
        PlayerBusted,
        DealerBusted,
        DealerBlackjack,
        Push,
        None
    }

    public class BlackjackGameResult
    { 
        public string Result { get; set; }
        public decimal Bet { get; set; }
        public string InitialDecision { get; set; }
        public decimal EndBalance { get; set; }
        public List<int> PlayerCards { get; set; }
        public List<int> DealerCards { get; set; }
        public int PlayerTotal { get; set; }   
        public int DealerTotal { get; set; }

        public string GetResultString(Result result)
        {
            switch (result)
            {
                case Models.Result.Blackjack:
                    return "Blackjack";
                case Models.Result.PlayerWon:
                    return "Player won";
                case Models.Result.DealerWon:
                    return "Dealer won";
                case Models.Result.PlayerBusted:
                    return "Player busted";
                case Models.Result.DealerBusted:
                    return "Dealer busted";
                case Models.Result.Push:
                    return "Push";
                default:
                    return "Unknown";
            }
        }

        public BlackjackGameResult(Result result, decimal bet, Decision initialDecision, decimal endBalance, List<int> playerCards, List<int> dealerCards, int playerTotal, int dealerTotal)
        {
            Result = result.ToString();
            Bet = bet;
            InitialDecision = initialDecision.ToString();
            EndBalance = endBalance;
            PlayerCards = playerCards;
            DealerCards = dealerCards;
            PlayerTotal = playerTotal;
            DealerTotal = dealerTotal;
        }
    }
}
