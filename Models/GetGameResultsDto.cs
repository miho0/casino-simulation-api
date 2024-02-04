namespace CasinoSimulationApi.Models
{
    public class GetGameResultsDto
    {
        public decimal InitialBalance { get; set; }
        public decimal BettingAmount { get; set; }
        public decimal Goal { get; set; }

        public GetGameResultsDto(decimal initialBalance, decimal bettingAmount, decimal goal)
        {
            InitialBalance = initialBalance;
            BettingAmount = bettingAmount;
            Goal = goal;
        }
    }
}
